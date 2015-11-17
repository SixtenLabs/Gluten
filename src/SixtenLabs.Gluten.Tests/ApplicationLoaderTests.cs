using Xunit;
using FluentAssertions;
using NSubstitute;

using System.Windows;

namespace SixtenLabs.Gluten.Tests
{
	public class subjectTests
	{
		public ApplicationLoader NewSubjectUnderTest()
		{
			// This will set up the URI registrations for the pack:// and application
			var a = Application.Current;

			var subject = new ApplicationLoader();

			return subject;
		}

		[Fact]
		public void ConstructorSetsResourceDictionary()
		{
			var subject = NewSubjectUnderTest();

			subject.MergedDictionaries.Count.Should().Be(1);

			var dict = subject.MergedDictionaries[0];

			dict.Source.AbsoluteUri.Should().Be("pack://application:,,,/SixtenLabs.Gluten;component/Xaml/GlutenResourceDictionary.xaml");
		}

		[Fact]
		public void UnloadsResourceDictionaryIfRequested()
		{
			var subject = NewSubjectUnderTest();

			subject.LoadResources = false;

			subject.MergedDictionaries.Count.Should().Be(0);
		}

		[Fact]
		public void CallsSetupOnBootstrapper()
		{
			var subject = NewSubjectUnderTest();

			var bootstrapper = Substitute.For<IBootstrapper>();
			subject.Bootstrapper = bootstrapper;

			bootstrapper.Received().Setup(Application.Current);
		}

		[Fact]
		public void LoadStyletResourcesReturnsCorrectValue()
		{
			var subject = NewSubjectUnderTest();

			subject.LoadResources.Should().BeTrue();
			subject.LoadResources = false;

			subject.LoadResources.Should().BeFalse();
		}

		[Fact]
		public void BootstrapperReturnsCorrectValue()
		{
			var subject = NewSubjectUnderTest();

			subject.Bootstrapper.Should().BeNull();
			var bootstrapper = Substitute.For<IBootstrapper>();

			subject.Bootstrapper = bootstrapper;

			subject.Bootstrapper.Should().Be(bootstrapper);
		}
	}
}
