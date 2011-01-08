using GLenum=System.UInt64;
using GLsizei=System.Int64;

namespace Joddes.CS.Html5
{
	[Hidden]
	public class WebGLRenderingContext
	{
		public GLenum DEPTH_BUFFER_BIT;
		public GLenum STENCIL_BUFFER_BIT;
		public GLenum COLOR_BUFFER_BIT;
	
		public GLenum POINTS;
		public GLenum LINES;
		public GLenum LINES_LOOP;
		public GLenum LINE_STRIP;
		public GLenum TRIANGLES;
		public GLenum TRIANGLE_STRIP;
		public GLenum TRIANGLE_FAN;
        public GLenum ARRAY_BUFFER;
        public GLenum STATIC_DRAW;
		
		public readonly HTMLCanvasElement canvas;
		
		public WebGLContextAttributes getContextAttributes ()
		{
			throw new System.NotSupportedException();
		}
		
		public void activeTexture (int texture)
		{
		}
		
		public void attachShader (WebGLProgram program, WebGLShader shader)
		{
		}
		
		public void viewport (int x, int y, int width, int height)
		{
		}

        public WebGLBuffer createBuffer ()
        {
            throw new System.NotSupportedException ();
        }

        public void bindBuffer (GLenum target, WebGLBuffer buffer)
        {
            throw new System.NotSupportedException ();
        }

        public void bufferData (GLenum target, GLsizei size, GLenum usage)
        {
        }

        public void bufferData (GLenum target, ArrayBufferView data, GLenum usage)
        {
        }

        public void bufferData (GLenum target, ArrayBuffer data, GLenum usage)
        {
        }
    }
}