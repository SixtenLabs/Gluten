using Xunit;
using FluentAssertions;

using System.Windows;
using System;
using System.Collections.Generic;

namespace SixtenLabs.Gluten.Tests
{
	public class BoolToVisibilityConverterTests
	{
		public BoolToVisibilityConverter NewSubjectUnderTest()
		{
			var subject = new BoolToVisibilityConverter();

			return subject;
		}

		[Fact]
		public void InstanceReturnsSingleton()
		{
			var subject = NewSubjectUnderTest();

			BoolToVisibilityConverter.Instance.Should().NotBeNull();
			BoolToVisibilityConverter.Instance.Should().Be(BoolToVisibilityConverter.Instance);
			BoolToVisibilityConverter.Instance.TrueVisibility.Should().Be(Visibility.Visible);
			BoolToVisibilityConverter.Instance.FalseVisibility.Should().Be(Visibility.Collapsed);
		}

		[Fact]
		public void InverseInstanceReturnsSingleton()
		{
			var subject = NewSubjectUnderTest();

			BoolToVisibilityConverter.InverseInstance.Should().NotBeNull();
			BoolToVisibilityConverter.InverseInstance.Should().Be(BoolToVisibilityConverter.InverseInstance);
			BoolToVisibilityConverter.InverseInstance.TrueVisibility.Should().Be(Visibility.Collapsed);
			BoolToVisibilityConverter.InverseInstance.FalseVisibility.Should().Be(Visibility.Visible);
		}

		[Fact]
		public void ConvertReturnsSetFalseVisibility()
		{
			var subject = NewSubjectUnderTest();

			subject.FalseVisibility = Visibility.Visible;

			var result = subject.Convert(false, null, null, null);

			result.Should().Be(Visibility.Visible);
		}

		[Theory]
		[InlineData(1, Visibility.Visible)]
		[InlineData(1u, Visibility.Visible)]
		[InlineData(1L, Visibility.Visible)]
		[InlineData(1Lu, Visibility.Visible)]
		//[InlineData(1m, Visibility.Visible)]
		[InlineData(1.0, Visibility.Visible)]
		[InlineData(1.0f, Visibility.Visible)]
		[InlineData((short)1, Visibility.Visible)]
		[InlineData((ushort)1, Visibility.Visible)]
		[InlineData((byte)1, Visibility.Visible)]
		[InlineData((sbyte)1, Visibility.Visible)]
		[InlineData("Hello", Visibility.Visible)]
		[InlineData(true, Visibility.Visible)]
		[InlineData(0, Visibility.Collapsed)]
		[InlineData(0u, Visibility.Collapsed)]
		[InlineData(0L, Visibility.Collapsed)]
		[InlineData(0Lu, Visibility.Collapsed)]
		//[InlineData(1m, Visibility.Visible)]
		[InlineData(0.0, Visibility.Collapsed)]
		[InlineData(0.0f, Visibility.Collapsed)]
		[InlineData((short)0, Visibility.Collapsed)]
		[InlineData((ushort)0, Visibility.Collapsed)]
		[InlineData((byte)0, Visibility.Collapsed)]
		[InlineData((sbyte)0, Visibility.Collapsed)]
		[InlineData(0, Visibility.Collapsed)]
		[InlineData(null, Visibility.Collapsed)]
		[InlineData("", Visibility.Collapsed)]
		[InlineData(false, Visibility.Collapsed)]
		public void Convert_Values_ReturnsCorrectVisibility(object value, Visibility expected)
		{
			var subject = NewSubjectUnderTest();

			subject.Convert(value, null, null, null).Should().Be(expected);
		}

		[Fact]
		public void ConvertTreatsNonNullAsTrue()
		{
			var subject = NewSubjectUnderTest();

			subject.Convert(new object(), null, null, null).Should().Be(Visibility.Visible);
		}

		[Fact]
		public void ConvertTreatsEmptyCollectionAsFalse()
		{
			var subject = NewSubjectUnderTest();

			subject.Convert(new int[0], null, null, null).Should().Be(Visibility.Collapsed);
			subject.Convert(new List<object>(), null, null, null).Should().Be(Visibility.Collapsed);
			subject.Convert(new Dictionary<string, string>(), null, null, null).Should().Be(Visibility.Collapsed);
		}

		[Fact]
		public void ConvertTreatsNonEmptyCollectionAsTrue()
		{
			var subject = NewSubjectUnderTest();

			subject.Convert(new int[1], null, null, null).Should().Be(Visibility.Visible);
			subject.Convert(new List<object>() { 3 }, null, null, null).Should().Be(Visibility.Visible);
			subject.Convert(new Dictionary<string, string>() { { "A", "B" } }, null, null, null).Should().Be(Visibility.Visible);
		}

		[Fact]
		public void ConvertTreatsRandomObjectAsTrue()
		{
			var subject = NewSubjectUnderTest();

			subject.Convert(typeof(int), null, null, null).Should().Be(Visibility.Visible);
		}

		[Fact]
		public void ConvertTreatsRandomValueTypeAsTrue()
		{
			var subject = NewSubjectUnderTest();

			subject.Convert(new KeyValuePair<int, int>(5, 5), null, null, null).Should().Be(Visibility.Visible);
		}

		[Fact]
		public void ConvertBackThrowsIfTargetTypeIsNotBool()
		{
			var subject = NewSubjectUnderTest();

			Action act = () => subject.ConvertBack(null, null, null, null);

			act.ShouldThrow<ArgumentException>();
		}

		[Fact]
		public void ConvertBackReturnsTrueIfValueIsTrueVisibility()
		{
			var subject = NewSubjectUnderTest();

			subject.TrueVisibility = Visibility.Hidden;
			var result = (bool)subject.ConvertBack(Visibility.Hidden, typeof(bool), null, null);

			result.Should().BeTrue();
		}

		[Fact]
		public void ConvertBackReturnsFalseIfValueIsFalseVisibility()
		{
			var subject = NewSubjectUnderTest();

			subject.FalseVisibility = Visibility.Hidden;

			var result = (bool)subject.ConvertBack(Visibility.Hidden, typeof(bool), null, null);

			result.Should().BeFalse();
		}

		[Fact]
		public void ConvertBackReturnsNullIfValueIsNotAVisibility()
		{
			var subject = NewSubjectUnderTest();

			var result = subject.ConvertBack(new object(), typeof(bool), null, null);

			result.Should().BeNull();
		}

		[Fact]
		public void ConvertBackReturnsNullIfValueIsNotAConfiguredVisibility()
		{
			var subject = NewSubjectUnderTest();

			var result = subject.ConvertBack(Visibility.Hidden, typeof(bool), null, null);

			result.Should().BeNull();
		}
	}
}
