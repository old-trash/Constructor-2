/*
	Полоса прокрутки для элемента вывода
*/


using System;
using System.Windows.Forms;


class ПолосаПрокрутки : VScrollBar
{
	public bool ИгнорироватьИзменениеЗначения = false;

	public ПолосаПрокрутки()
	{
		Dock = DockStyle.Right;
		SmallChange = LargeChange = 1;
		Восстановить();
	}

	public void Восстановить()
	{
		ИгнорироватьИзменениеЗначения = false;
		Minimum = Maximum = -1;
		Enabled = false;
	}

	protected override void OnValueChanged(EventArgs e)
	{
		if (ИгнорироватьИзменениеЗначения)
			return;
		if (Parent != null)
			Parent.Invalidate();
	}

	new public int Minimum
	{
		set
		{
			if (Maximum < value)
				Maximum = value;
			// В исходнике .Net Framework v2.0.40607 вместо
			// "this.Value = value" написано "this.value = value",
			// поэтому OnValueChanged не вызывается.
			// Здесь эта ошибка обходится.
			if (value > Value)
				Value = value;
			base.Minimum = value;
		}
		get
		{
			return base.Minimum;
		}
	}

	public int Максимум
	{
		set
		{
			if (value < 0)
			{
				Восстановить();
			}
			else
			{
				Minimum = 0;
				Maximum = value;
				if (Maximum > 0)
					Enabled = true;
			}
		}
		get
		{
			return Maximum;
		}
	}
}
