using OpenGL;
using System;
using Tao.FreeGlut;

namespace Tema3
{
    class Program
    {
        private static int width = 1920, height = 1080;
        private static ShaderProgram program;
        private static VBO<Vector3> paralelipiped, pyramide, cube, sphere;
        private static VBO<Vector3> paralelipipedColor, pyramideColor, cubeColor, sphereColor;
        private static VBO<Vector2> paralelipipedUV;
        private static VBO<uint> paralelipipedElements, pyramideElements, cubeElements, sphereElements;
        private static Texture crateTexture;
        private static System.Diagnostics.Stopwatch watch;
        private static float xangle, yangle, zangle;
        private static double movex, movey, i;
        private static bool autoRotate, lighting = true, fullscreen = false;
        private static bool left, right, up, down, zleft, zright, sus, jos, dreapta, stanga;

        static void Main(string[] args)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Glut.glutInitWindowSize(width, height);
            Glut.glutCreateWindow("TAP - Tema nr. 3 <>Baloi Razvan Dumitru<>");

            Glut.glutIdleFunc(OnRenderFrame);
            Glut.glutDisplayFunc(OnDisplay);

            Glut.glutKeyboardFunc(OnKeyboardDown);
            Glut.glutKeyboardUpFunc(OnKeyboardUp);

            Glut.glutCloseFunc(OnClose);
            Glut.glutReshapeFunc(OnReshape);

            Gl.Enable(EnableCap.DepthTest);

            program = new ShaderProgram(VertexShader, FragmentShader);

            program.Use();
            program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)width / height, 0.1f, 1000f));
            program["view_matrix"].SetValue(Matrix4.LookAt(new Vector3(0, 0, 10), Vector3.Zero, new Vector3(0, 1, 0)));

            program["light_direction"].SetValue(new Vector3(0, 0, 1));
            program["enable_lighting"].SetValue(lighting);

            crateTexture = new Texture("crate.jpg");

            paralelipiped = new VBO<Vector3>(new Vector3[]
            {
                new Vector3(0.5,       0.5,     -0.5), new Vector3(-0.5,        0.5,      -0.5), new Vector3(-0.5,         0.5,      1), new Vector3(0.5,      0.5,      1),         // top
                new Vector3(0.5,      -0.5,      1), new Vector3(-0.5,       -0.5,       1), new Vector3(-0.5,        -0.5,     -0.5), new Vector3(0.5,     -0.5,     -0.5),     // bottom
                new Vector3(0.5,       0.5,      1), new Vector3(-0.5,        0.5,       1), new Vector3(-0.5,        -0.5,      1), new Vector3(0.5,     -0.5,      1),         // front face
                new Vector3(0.5,      -0.5,     -0.5), new Vector3(-0.5,       -0.5,      -0.5), new Vector3(-0.5,         0.5,     -0.5), new Vector3(0.5,      0.5,     -0.5),     // back face
                new Vector3(-0.5,      0.5,      1), new Vector3(-0.5,        0.5,      -0.5), new Vector3(-0.5,        -0.5,     -0.5), new Vector3(-0.5,    -0.5,      1),     // left
                new Vector3(0.5,       0.5,     -0.5), new Vector3(0.5,         0.5,       1), new Vector3(0.5,         -0.5,      1), new Vector3(0.5,     -0.5,     -0.5)         // right
            
            });
            paralelipipedColor = new VBO<Vector3>(new Vector3[] {
                new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 1, 0),
                new Vector3(0, -1, 0), new Vector3(0, -1, 0), new Vector3(0, -1, 0), new Vector3(0, -1, 0),
                new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 1),
                new Vector3(0, 0, -1), new Vector3(0, 0, -1), new Vector3(0, 0, -1), new Vector3(0, 0, -1),
                new Vector3(-1, 0, 0), new Vector3(-1, 0, 0), new Vector3(-1, 0, 0), new Vector3(-1, 0, 0),
                new Vector3(1, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 0) });
            paralelipipedUV = new VBO<Vector2>(new Vector2[] {
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) });

            paralelipipedElements = new VBO<uint>(new uint[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 }, BufferTarget.ElementArrayBuffer);


            pyramide = new VBO<Vector3>(new Vector3[] {
                    new Vector3(0,  0.5,   0), new Vector3(-0.5,    -0.5,      0.5),new Vector3(0.5,      -0.5,      0.5), //Front Face
                    new Vector3(0,  0.5,   0), new Vector3(0.5,     -0.5,      0.5),new Vector3(0.5,      -0.5,     -0.5), //Right Face
                    new Vector3(0,  0.5,   0), new Vector3(0.5,     -0.5,     -0.5),new Vector3(-0.5,     -0.5,     -0.5), //Back Face
                    new Vector3(0,  0.5,   0), new Vector3(-0.5,    -0.5,     -0.5),new Vector3(-0.5,     -0.5,      0.5), //left Face
            });
            pyramideColor = new VBO<Vector3>(new Vector3[] {
                    new Vector3(1,      0,      0), new Vector3(0,      1,      0), new Vector3(0,      0,      1),
                    new Vector3(1,      0,      0), new Vector3(0,      0,      1), new Vector3(0,      1,      0),
                    new Vector3(1,      0,      0), new Vector3(0,      1,      0), new Vector3(0,      0,      1),
                    new Vector3(1,      0,      0), new Vector3(0,      0,      1), new Vector3(0,      1,      0),
            });
            pyramideElements = new VBO<uint>(new uint[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, BufferTarget.ElementArrayBuffer);

            // create a cub with vertices and colors
            cube = new VBO<Vector3>(new Vector3[]
            {
                new Vector3(0.5,       0.5,     -0.5),new Vector3(-0.5,      0.5,      -0.5),new Vector3(-0.5,      0.5,      0.5),new Vector3(0.5,      0.5,       0.5),
                new Vector3(0.5,      -0.5,      0.5),new Vector3(-0.5,     -0.5,       0.5),new Vector3(-0.5,     -0.5,     -0.5),new Vector3(0.5,     -0.5,      -0.5),
                new Vector3(0.5,       0.5,      0.5),new Vector3(-0.5,      0.5,       0.5),new Vector3(-0.5,     -0.5,      0.5),new Vector3(0.5,     -0.5,       0.5),
                new Vector3(0.5,      -0.5,     -0.5),new Vector3(-0.5,     -0.5,      -0.5),new Vector3(-0.5,      0.5,     -0.5),new Vector3(0.5,      0.5,      -0.5),
                new Vector3(-0.5,      0.5,      0.5),new Vector3(-0.5,      0.5,      -0.5),new Vector3(-0.5,     -0.5,     -0.5),new Vector3(-0.5,    -0.5,       0.5),
                new Vector3(0.5,       0.5,     -0.5),new Vector3(0.5,       0.5,       0.5),new Vector3(0.5,      -0.5,      0.5),new Vector3(0.5,     -0.5,      -0.5),

            });

            cubeColor = new VBO<Vector3>(new Vector3[]
            {

                new Vector3(0,      1,      0),new Vector3(0,       1,      0), new Vector3(0,      1,      0), new Vector3(0,      1,      0),
                new Vector3(1,      0.5,    0),new Vector3(1,       0.5,    0), new Vector3(1,      0.5,    0), new Vector3(1,      0.5,    0),
                new Vector3(1,      0,      0),new Vector3(1,       0,      0), new Vector3(1,      0,      0), new Vector3(1,      0,      0),
                new Vector3(1,      1,      0),new Vector3(1,       1,      0), new Vector3(1,      1,      0), new Vector3(1,      1,      0),
                new Vector3(0,      0,      1),new Vector3(0,       0,      1), new Vector3(0,      0,      1), new Vector3(0,      0,      1),
                new Vector3(1,      0,      1),new Vector3(1,       0,      1), new Vector3(1,      0,      1), new Vector3(1,      0,      1)
                });

            cubeElements = new VBO<uint>(new uint[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 }, BufferTarget.ElementArrayBuffer);

            // Create a sphere with vertices and colors
            sphere = new VBO<Vector3>(new Vector3[]
            {
                new Vector3(0.5,       0.5,     -0.5),new Vector3(-0.5,      0.5,      -0.5),new Vector3(-0.5,      0.5,      0.5),new Vector3(0.5,      0.5,       0.5),
                new Vector3(0.5,      -0.5,      0.5),new Vector3(-0.5,     -0.5,       0.5),new Vector3(-0.5,     -0.5,     -0.5),new Vector3(0.5,     -0.5,      -0.5),
                new Vector3(0.5,       0.5,      0.5),new Vector3(-0.5,      0.5,       0.5),new Vector3(-0.5,     -0.5,      0.5),new Vector3(0.5,     -0.5,       0.5),
                new Vector3(0.5,      -0.5,     -0.5),new Vector3(-0.5,     -0.5,      -0.5),new Vector3(-0.5,      0.5,     -0.5),new Vector3(0.5,      0.5,      -0.5),
                new Vector3(-0.5,      0.5,      0.5),new Vector3(-0.5,      0.5,      -0.5),new Vector3(-0.5,     -0.5,     -0.5),new Vector3(-0.5,    -0.5,       0.5),
                new Vector3(0.5,       0.5,     -0.5),new Vector3(0.5,       0.5,       0.5),new Vector3(0.5,      -0.5,      0.5),new Vector3(0.5,     -0.5,      -0.5),


            });

            sphereColor = new VBO<Vector3>(new Vector3[]
            {

                new Vector3(0,      1,      0),new Vector3(0,       1,      0), new Vector3(0,      1,      0), new Vector3(0,      1,      0),
                new Vector3(1,      0.5,    0),new Vector3(1,       0.5,    0), new Vector3(1,      0.5,    0), new Vector3(1,      0.5,    0),
                new Vector3(1,      0,      0),new Vector3(1,       0,      0), new Vector3(1,      0,      0), new Vector3(1,      0,      0),
                new Vector3(1,      1,      0),new Vector3(1,       1,      0), new Vector3(1,      1,      0), new Vector3(1,      1,      0),
                new Vector3(0,      0,      1),new Vector3(0,       0,      1), new Vector3(0,      0,      1), new Vector3(0,      0,      1),
                new Vector3(1,      0,      1),new Vector3(1,       0,      1), new Vector3(1,      0,      1), new Vector3(1,      0,      1),
                });

            sphereElements = new VBO<uint>(new uint[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 }, BufferTarget.ElementArrayBuffer);

            watch = System.Diagnostics.Stopwatch.StartNew();

            Glut.glutMainLoop();
        }

        private static void OnClose()
        {
            pyramide.Dispose();
            pyramideColor.Dispose();
            pyramideElements.Dispose();

            sphere.Dispose();
            sphereColor.Dispose();
            sphereElements.Dispose();
            program.DisposeChildren = true;
            program.Dispose();

            cube.Dispose();
            cubeColor.Dispose();
            cubeElements.Dispose();
            program.DisposeChildren = true;
            program.Dispose();


            paralelipiped.Dispose();
            paralelipipedColor.Dispose();
            paralelipipedUV.Dispose();
            paralelipipedElements.Dispose();
            crateTexture.Dispose();
            program.DisposeChildren = true;
            program.Dispose();
        }

        private static void OnDisplay()
        {

        }

        private static void OnRenderFrame()
        {
            watch.Stop();
            float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
            watch.Restart();

            // perform rotation of the paralelipiped depending on the keyboard state
            if (autoRotate)
            {
                xangle += deltaTime / 2;
                yangle += deltaTime;
                zangle += deltaTime / 3;
                //movex += deltaTime;
                //movex -= deltaTime / 2;
                //movey += deltaTime;
                //movey -= deltaTime / 2;
                //movex += deltaTime;



                /*
                if (movex < 1 && movey == 0) movex += deltaTime;
                else if (movex > 0 && movey < 1) movey += deltaTime;
                else if (movey == 1 && movex < 1) movex -= deltaTime;
                else movey -= deltaTime;
                */

                i = 0.0001;

                if (movex < 1.0001 && movey < 2.0001) { movex += i; }
                else if (movex < 1.0002 && movey < 2.0001) { movey += i; }
                else { movex -= i; movey -= i; }
                //else{                    movey -= i;                }

                //Console.WriteLine(deltaTime.ToString());
                Console.WriteLine("Movey:");
                Console.WriteLine(movey);
                Console.WriteLine("Movex:");
                Console.WriteLine(movex);

            }
            if (right) yangle += deltaTime;
            if (left) yangle -= deltaTime;
            if (up) xangle -= deltaTime;
            if (down) xangle += deltaTime;
            if (zright) zangle += deltaTime;
            if (zleft) zangle -= deltaTime;
            if (sus) movex += deltaTime;
            if (jos) movex -= deltaTime;
            if (dreapta) movey += deltaTime;
            if (stanga) movey -= deltaTime;

            // set up the viewport and clear the previous depth and color buffers
            Gl.Viewport(0, 0, width, height);
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // make sure the shader program and texture are being used
            Gl.UseProgram(program);
            Gl.BindTexture(crateTexture);


            //set up the model matrix and draw the cube
            program["model_matrix"].SetValue(Matrix4.CreateRotationY(yangle) * Matrix4.CreateRotationX(xangle) * Matrix4.CreateRotationZ(zangle) * Matrix4.CreateTranslation(new Vector3(1f, movex, movey)));

            Gl.BindBufferToShaderAttribute(cube, program, "vertexPosition");
            Gl.BindBufferToShaderAttribute(cubeColor, program, "vertexColor");
            Gl.BindBuffer(cubeElements);

            Gl.DrawElements(BeginMode.Quads, cubeElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);


            // set up the model matrix and draw the paralelipiped
            program["model_matrix"].SetValue(Matrix4.CreateRotationY(yangle) * Matrix4.CreateRotationX(xangle) * Matrix4.CreateRotationZ(zangle) * Matrix4.CreateTranslation(new Vector3(-1f, movex, movey)));
            program["enable_lighting"].SetValue(lighting);

            Gl.BindBufferToShaderAttribute(paralelipiped, program, "vertexPosition");
            Gl.BindBufferToShaderAttribute(paralelipipedColor, program, "vertexNormal");
            Gl.BindBufferToShaderAttribute(paralelipipedUV, program, "vertexUV");
            Gl.BindBuffer(paralelipipedElements);

            Gl.DrawElements(BeginMode.Quads, paralelipipedElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

            // set up the mode matrix and draw the piramid
            program["model_matrix"].SetValue(Matrix4.CreateRotationY(yangle) * Matrix4.CreateRotationX(xangle) * Matrix4.CreateRotationZ(zangle) * Matrix4.CreateTranslation(new Vector3(-2.5f, movex, movey)));

            Gl.BindBufferToShaderAttribute(pyramide, program, "vertexPosition");
            Gl.BindBufferToShaderAttribute(pyramideColor, program, "vertexColor");
            Gl.BindBuffer(pyramideElements);

            Gl.DrawElements(BeginMode.Triangles, pyramideElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

            // set up the mode matrix and draw the sphere
            program["model_matrix"].SetValue(Matrix4.CreateRotationY(yangle) * Matrix4.CreateRotationX(xangle) * Matrix4.CreateRotationZ(zangle) * Matrix4.CreateTranslation(new Vector3(2.5f, movex, movey)));

            Gl.BindBufferToShaderAttribute(sphere, program, "vertexPosition");
            Gl.BindBufferToShaderAttribute(sphereColor, program, "vertexColor");
            Gl.BindBuffer(sphereElements);

            Gl.DrawElements(BeginMode.Quads, sphereElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

            Glut.glutSwapBuffers();
        }

        private static void OnReshape(int width, int height)
        {
            Program.width = width;
            Program.height = height;

            program.Use();
            program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)width / height, 0.1f, 1000f));
        }

        private static void OnKeyboardDown(byte key, int x, int y)
        {
            if (key == 'w') up = true;
            else if (key == 's') down = true;
            else if (key == 'd') right = true;
            else if (key == 'a') left = true;
            else if (key == 'z') zright = true;
            else if (key == 'x') zleft = true;
            else if (key == 'c') sus = true;
            else if (key == 'v') jos = true;
            else if (key == 'b') dreapta = true;
            else if (key == 'n') stanga = true;
            else if (key == 27) Glut.glutLeaveMainLoop();
        }

        private static void OnKeyboardUp(byte key, int x, int y)
        {
            if (key == 'w') up = false;
            else if (key == 's') down = false;
            else if (key == 'd') right = false;
            else if (key == 'a') left = false;
            else if (key == 'r') autoRotate = !autoRotate;
            else if (key == 'l') lighting = !lighting;
            else if (key == 'z') zright = false;
            else if (key == 'x') zleft = false;
            else if (key == 'c') sus = false;
            else if (key == 'v') jos = false;
            else if (key == 'b') dreapta = false;
            else if (key == 'n') stanga = false;
            else if (key == 'f')
            {
                fullscreen = !fullscreen;
                if (fullscreen) Glut.glutFullScreen();
                else
                {
                    Glut.glutPositionWindow(0, 0);
                    Glut.glutReshapeWindow(1920, 1080);
                }
            }
        }

        public static string VertexShader = @"
#version 130

in vec3 vertexPosition;
in vec3 vertexNormal;
in vec2 vertexUV;

out vec3 normal;
out vec2 uv;

uniform mat4 projection_matrix;
uniform mat4 view_matrix;
uniform mat4 model_matrix;

void main(void)
{
    normal = normalize((model_matrix * vec4(floor(vertexNormal), 0)).xyz);
    uv = vertexUV;

    gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vertexPosition, 1);
}
";

        public static string FragmentShader = @"
#version 130

uniform sampler2D texture;
uniform vec3 light_direction;
uniform bool enable_lighting;

in vec3 normal;
in vec2 uv;

out vec4 fragment;

void main(void)
{
    float diffuse = max(dot(normal, light_direction), 0);
    float ambient = 0.3;
    float lighting = (enable_lighting ? max(diffuse, ambient) : 1);

    fragment = lighting * texture2D(texture, uv);
}
";
    }
}
