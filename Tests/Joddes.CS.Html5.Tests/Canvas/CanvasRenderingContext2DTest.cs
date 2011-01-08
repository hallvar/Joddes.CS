using System;
using Joddes.CS.TestFramework;
using Joddes.CS.Html5;

namespace Joddes.CS.Html5.Tests.Canvas
{
    public class CanvasRenderingContext2DTest : Test
    {
        HTMLCanvasElement canvas;
        CanvasRenderingContext2D ctx;

        public override void Setup ()
        {
            canvas = (HTMLCanvasElement)Window.Self.Document.CreateElement ("canvas");

            Window.Self.Document.Body.AppendChild (canvas);

            ctx = (CanvasRenderingContext2D)canvas.GetContext ("2d");
        }

        public override void TearDown ()
        {
            Window.Self.Document.Body.RemoveChild(canvas);
        }

        [Test]
        public void When_creating_canvas_then_getcontext_2D_gives_a_non_null_context ()
        {
            Assert.NotNull (ctx);
        }

        [Test]
        public void When_fillRect_then ()
        {
            ctx.fillRect (0, 0, 100, 100);
        }
    }
}