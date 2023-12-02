﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminal.Gui;

namespace UICatalog.Scenarios;

[ScenarioMetadata (Name: "Sliders", Description: "Demonstrates the Slider view.")]
[ScenarioCategory ("Controls")]
public class Sliders : Scenario {
	public override void Setup ()
	{
		MakeSliders (Win, new List<object> { 500, 1000, 1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000 });

		#region configView

		var configView = new FrameView {
			Title = "Configuration",
			X = Pos.Percent (50),
			Y = 0,
			Width = Dim.Fill (),
			Height = Dim.Fill (),
			ColorScheme = Colors.Dialog
		};

		Win.Add (configView);

		#region Config Slider

		var slider = new Slider<string> () {
			Title = "Options",
			X = Pos.Center (),
			Y = 0,
			Type = SliderType.Multiple,
			Width = Dim.Fill (),
			AllowEmpty = true,
			BorderStyle = LineStyle.Single
		};

		slider.Style.SetChar.Attribute = new Terminal.Gui.Attribute (Color.BrightGreen, Color.Black);
		slider.Style.LegendAttributes.SetAttribute = new Terminal.Gui.Attribute (Color.Green, Color.Black);

		slider.Options = new List<SliderOption<string>> {
					new SliderOption<string>{
						Legend="Legends"
					},
					new SliderOption<string>{
						Legend="RangeAllowSingle"
					},
					new SliderOption<string>{
						Legend="Spacing"
					}
				};

		configView.Add (slider);

		slider.OptionsChanged += (sender, e) => {
			foreach (var s in Win.Subviews.OfType<Slider> ()) {
				if (e.Options.ContainsKey (0))
					s.ShowLegends = true;
				else
					s.ShowLegends = false;

				if (e.Options.ContainsKey (1))
					s.RangeAllowSingle = true;
				else
					s.RangeAllowSingle = false;

				if (e.Options.ContainsKey (2))
					s.ShowSpacing = true;
				else
					s.ShowSpacing = false;
			}
			Win.LayoutSubviews ();
		};
		slider.SetOption (0);
		slider.SetOption (1);

		#endregion

		#region InnerSpacing Input
		// var innerspacing_slider = new Slider<string> ("Innerspacing", new List<string> { "Auto", "0", "1", "2", "3", "4", "5" }) {
		// 	X = Pos.Center (),
		// 	Y = Pos.Bottom (slider) + 1
		// };

		// innerspacing_slider.SetOption (0);

		// configView.Add (innerspacing_slider);

		// innerspacing_slider.OptionsChanged += (options) => {
		// 	foreach (var s in leftView.Subviews.OfType<Slider> () ()) {
		// 		if (options.ContainsKey (0)) { }
		// 		//s.la = S.SliderLayout.Auto;
		// 		else {
		// 			s.InnerSpacing = options.Keys.First () - 1;
		// 		}
		// 	}
		// };
		#endregion

		#region Slider Orientation Slider

		var slider_orientation_slider = new Slider<string> (new List<string> { "Horizontal", "Vertical" }) {
			Title = "Slider Orientation",
			X = 0,
			Y = Pos.Bottom (slider) + 1,
			Width = Dim.Fill (),
			BorderStyle = LineStyle.Single
		};

		slider_orientation_slider.SetOption (0);

		configView.Add (slider_orientation_slider);

		slider_orientation_slider.OptionsChanged += (sender, e) => {

			View prev = null;
			foreach (var s in Win.Subviews.OfType<Slider> ()) {

				if (e.Options.ContainsKey (0)) {
					s.Orientation = Orientation.Horizontal;

					s.AdjustBestHeight ();
					s.Width = Dim.Percent (50);

					s.Style.SpaceChar = new Cell () { Rune = CM.Glyphs.HLine };

					if (prev == null) {
						s.LayoutStyle = LayoutStyle.Absolute;
						s.Y = 0;
						s.LayoutStyle = LayoutStyle.Computed;
					} else {
						s.Y = Pos.Bottom (prev) + 1;
					}
					s.X = 0;
					prev = s;

				} else if (e.Options.ContainsKey (1)) {
					s.Orientation = Orientation.Vertical;

					s.AdjustBestWidth ();
					s.Height = Dim.Fill ();

					s.Style.SpaceChar = new Cell () { Rune = CM.Glyphs.VLine };


					if (prev == null) {
						s.LayoutStyle = LayoutStyle.Absolute;
						s.X = 0;
						s.LayoutStyle = LayoutStyle.Computed;
					} else {
						s.X = Pos.Right (prev) + 2;
					}
					s.Y = 0;
					prev = s;
				}
			}
			Win.LayoutSubviews ();
		};

		#endregion

		#region Legends Orientation Slider

		var legends_orientation_slider = new Slider<string> (new List<string> { "Horizontal", "Vertical" }) {
			Title = "Legends Orientation",
			X = Pos.Center (),
			Y = Pos.Bottom (slider_orientation_slider) + 1,
			Width = Dim.Fill (),
			BorderStyle = LineStyle.Single
		};

		legends_orientation_slider.SetOption (0);

		configView.Add (legends_orientation_slider);

		legends_orientation_slider.OptionsChanged += (sender, e) => {
			foreach (var s in Win.Subviews.OfType<Slider> ()) {
				if (e.Options.ContainsKey (0))
					s.LegendsOrientation = Orientation.Horizontal;
				else if (e.Options.ContainsKey (1))
					s.LegendsOrientation = Orientation.Vertical;
			}
			Win.LayoutSubviews ();
		};

		#endregion

		#region Color Slider

		var sliderColor = new Slider<(Color, Color)> () {
			Title = "Color",
			X = Pos.Center (),
			Y = Pos.Bottom (legends_orientation_slider) + 1,
			Type = SliderType.Single,
			Width = Dim.Fill (),
			BorderStyle = LineStyle.Single,
			AllowEmpty = false
		};

		sliderColor.Style.SetChar.Attribute = new Terminal.Gui.Attribute (Color.BrightGreen, Color.Black);
		sliderColor.Style.LegendAttributes.SetAttribute = new Terminal.Gui.Attribute (Color.Green, Color.Blue);

		sliderColor.LegendsOrientation = Orientation.Vertical;
		var colorOptions = new List<SliderOption<(Color, Color)>> ();
		foreach (var colorIndex in Enum.GetValues<ColorName> ()) {
			var colorName = colorIndex.ToString ();
			colorOptions.Add (new SliderOption<(Color, Color)> {
				Data = (new Color((ColorName)colorIndex), Win.GetNormalColor ().Background),
				Legend = colorName,
				LegendAbbr = (Rune)colorName [0],
			});
		}
		sliderColor.Options = colorOptions;

		configView.Add (sliderColor);

		sliderColor.OptionsChanged += (sender, e) => {
			if (e.Options.Count != 0) {
				var data = e.Options.First ().Value.Data;

				foreach (var s in Win.Subviews.OfType<Slider> ()) {
					s.Style.OptionChar.Attribute = new Attribute (data.Item1, data.Item2);
					s.Style.SetChar.Attribute = new Attribute (data.Item1, data.Item2);
					s.Style.LegendAttributes.SetAttribute = new Attribute (data.Item1, Win.GetNormalColor ().Background);
					s.Style.RangeChar.Attribute = new Attribute (data.Item1, Win.GetNormalColor ().Background);
					s.Style.SpaceChar.Attribute = new Attribute (data.Item1, Win.GetNormalColor ().Background);
					s.Style.LegendAttributes.NormalAttribute = new Attribute (data.Item1, Win.GetNormalColor ().Background);
					// Here we can not call SetNeedDisplay(), because the OptionsChanged was triggered by Key Pressing,
					// that internaly calls SetNeedDisplay.
				}
			} else {
				foreach (var s in Win.Subviews.OfType<Slider> ()) {
					s.Style.SetChar.Attribute = null;
					s.Style.LegendAttributes.SetAttribute = null;
					s.Style.RangeChar.Attribute = null;
				}
			}
		};

		// Set option after Eventhandler def, so it updates the sliders color.
		// sliderColor.SetOption (2);

		#endregion

		#endregion

		Win.FocusFirst ();

	}

	public void MakeSliders (View v, List<object> options)
	{
		var types = Enum.GetValues (typeof (SliderType)).Cast<SliderType> ().ToList ();

		Slider prev = null;

		foreach (var type in types) {
			var view = new Slider (options, Orientation.Horizontal) {
				Title = type.ToString (),
				X = 0,
				//X = Pos.Right (view) + 1,
				Y = prev == null ? 0 : Pos.Bottom (prev),
				//Y = Pos.Center (),
				Width = Dim.Percent (50),
				BorderStyle = LineStyle.Single,
				Type = type,
				LegendsOrientation = Orientation.Horizontal,
				AllowEmpty = true,
			};
			v.Add (view);
			prev = view;
		};

		var singleOptions = new List<object> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39 };

		var single = new Slider (singleOptions, Orientation.Horizontal) {
			Title = "Actual slider",
			X = 0,
			//X = Pos.Right (view) + 1,
			Y = prev == null ? 0 : Pos.Bottom (prev),
			//Y = Pos.Center (),
			Type = SliderType.Single,
			//BorderStyle = LineStyle.Single,
			LegendsOrientation = Orientation.Horizontal,
			Width = Dim.Percent (50),
			AllowEmpty = false,
			//ShowSpacing = true
		};

		single.LayoutStarted += (s, e) => {
			if (single.Orientation == Orientation.Horizontal) {
				single.Style.SpaceChar = new Cell () { Rune = CM.Glyphs.HLine };
				single.Style.OptionChar = new Cell () { Rune = CM.Glyphs.HLine };
			} else {
				single.Style.SpaceChar = new Cell () { Rune = CM.Glyphs.VLine };
				single.Style.OptionChar = new Cell () { Rune = CM.Glyphs.VLine };
			}
		};
		single.Style.SetChar = new Cell () { Rune = CM.Glyphs.ContinuousMeterSegment };
		single.Style.DragChar = new Cell () { Rune = CM.Glyphs.ContinuousMeterSegment };

		v.Add (single);

		var label = new Label () {
			X = 0,
			Y = Pos.Bottom (single),
			Height = 1,
			Width = Dim.Width (single),
			Text = $"{single.GetSetOptions ().FirstOrDefault ()}"
		};
		single.OptionsChanged += (s, e) => {
			label.Text = $"{e.Options.FirstOrDefault ().Key}";
		};

		v.Add (label);
	}
}