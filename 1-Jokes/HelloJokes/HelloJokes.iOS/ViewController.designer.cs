// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace HelloJokes.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton getJoke { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel jokeText { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (getJoke != null) {
                getJoke.Dispose ();
                getJoke = null;
            }

            if (jokeText != null) {
                jokeText.Dispose ();
                jokeText = null;
            }
        }
    }
}