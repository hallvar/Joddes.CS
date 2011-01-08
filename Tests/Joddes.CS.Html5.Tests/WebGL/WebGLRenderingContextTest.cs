using System;
using Joddes.CS.TestFramework;
using Joddes.CS.Html5;

namespace Joddes.CS.Html5.Tests.WebGL
{
    public class WebGLRenderingContextTest : Test
    {
        HTMLCanvasElement canvas;
        WebGLRenderingContext gl;

        public override void Setup ()
        {
            canvas = (HTMLCanvasElement)Window.Self.Document.CreateElement ("canvas");

            Window.Self.Document.Body.AppendChild (canvas);

            gl = (WebGLRenderingContext)canvas.GetContext ("webkit-3d");
        }

        public override void TearDown ()
        {
            Window.Self.Document.Body.RemoveChild(canvas);
        }

        [Test]
        public void When_creating_canvas_then_getcontext_webgl_gives_a_non_null_context ()
        {
            Assert.NotNull (gl);
        }

        [Test]
        public void When_then ()
        {
            gl.viewport (0, 0, canvas.width, canvas.height);
            var triangleVertexPositionBuffer = gl.createBuffer ();
            gl.bindBuffer (gl.ARRAY_BUFFER, triangleVertexPositionBuffer);

            var vertices = new double[] {
                 0.0, 1.0, 0.0,
                -1.0, -1.0, 0.0,
                 1.0, -1.0, 0.0
            };

            //gl.bufferData (gl.ARRAY_BUFFER, new WebGLFloatArray (vertices), gl.STATIC_DRAW);
        }
    }
}