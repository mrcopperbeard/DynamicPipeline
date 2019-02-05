﻿using System;
using System.Collections.Generic;

using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace DynamicPipeline.Tests
{
	[TestFixture]
	public class StringPipelineTests
	{
		private IKernel _kernel;

		private IPipeline<string> _pipeline;

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			var ninjectSettings = new NinjectSettings
			{
				LoadExtensions = false
			};

			_kernel = new StandardKernel(ninjectSettings);
			_pipeline = new Pipeline<string>(_kernel);
		}

		[TearDown]
		public void TearDown()
		{
			_kernel.Unbind<IHandler<string>>();
		}

		[Test]
		public void BothSuccessExecutionTest()
		{
			// arrange
			var configuration = new Dictionary<string, IHandlerBehavior>
			{
				{ "Foo", new HandlerBehavior() },
				{ "Bar", new HandlerBehavior() },
			};

			_kernel.Bind<IHandler<string>>().To<FooHandler>().Named("Foo");
			_kernel.Bind<IHandler<string>>().To<BarHandler>().Named("Bar");

			_pipeline.Configure(configuration);

			// act
			var result = _pipeline.Execute(new HandleContext<string>("Foo and Bar"));

			// assert
			result.Success.Should().BeTrue();
		}

		[Test]
		public void OnlyFooExecutionTest()
		{
			// arrange
			var configuration = new Dictionary<string, IHandlerBehavior>
			{
				{ "Foo", new HandlerBehavior() },
				{ "Bar", new HandlerBehavior() },
			};

			_kernel.Bind<IHandler<string>>().To<FooHandler>().Named("Foo");
			_kernel.Bind<IHandler<string>>().To<BarHandler>().Named("Bar");

			_pipeline.Configure(configuration);

			// act
			var result = _pipeline.Execute(new HandleContext<string>("Only Foo"));

			// assert
			result.Success.Should().BeFalse();
			result.Errors.Should().HaveCount(1);
		}

		[Test]
		public void BarHandleFooErrorsTest()
		{
			// arrange
			var configuration = new Dictionary<string, IHandlerBehavior>
			{
				{ "Foo", new HandlerBehavior() },
				{
					"Bar", new HandlerBehavior
					{
						Mode = HandleMode.OnError,
					}
				},
			};

			var barMock = new Mock<BarHandler>();
			var context = new HandleContext<string>("It will fail");

			barMock.Setup(b => b.Handle(context)).Returns(new HandleResult());

			_kernel.Bind<IHandler<string>>().To<FooHandler>().Named("Foo");
			_kernel.Bind<IHandler<string>>().ToMethod(ctx => barMock.Object).Named("Bar");

			_pipeline.Configure(configuration);

			// act
			var result = _pipeline.Execute(context);

			// assert
			result.Success.Should().BeFalse();
			result.Errors.Should().HaveCount(1);
			barMock.Verify(b => b.HandleError(context));
		}
	}
}
