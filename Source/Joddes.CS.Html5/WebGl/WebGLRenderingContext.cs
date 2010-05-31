namespace Joddes.CS.Html5
{
	[Hidden]
	public class WebGLRenderingContext
	{
		public int DEPTH_BUFFER_BIT;
		public int STENCIL_BUFFER_BIT;
		public int COLOR_BUFFER_BIT;
	
		public int POINTS;
		public int LINES;
		public int LINES_LOOP;
		public int LINE_STRIP;
		public int TRIANGLES;
		public int TRIANGLE_STRIP;
		public int TRIANGLE_FAN;
		
		public readonly HTMLCanvasElement canvas;
		
		public WebGLContextAttributes getContextAttributes ()
		{
			return null;
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
	}
}