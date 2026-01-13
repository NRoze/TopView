using TopView.Common.Infrastructure;
using Xunit;

namespace TopView.Tests.Common
{
	public class BaseNotifyTests
	{
		[Fact]
		public void SetProperty_UpdatesValueAndRaisesPropertyChanged()
		{
			var obj = new TestNotify();

			bool raised = false;
			obj.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(TestNotify.Value)) raised = true; };

			obj.SetValue(5);

			Assert.Equal(5, obj.Value);
			Assert.True(raised);
		}
	}

	class TestNotify : BaseNotify
	{
		private int _value;
		public int Value { get => _value; set => SetProperty(ref _value, value); }
		public void SetValue(int v) => Value = v;
	}
}
