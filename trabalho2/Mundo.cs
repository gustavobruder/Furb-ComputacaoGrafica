//TODO: testar se estes DEFINEs continuam funcionado
#define CG_Gizmo  // debugar gráfico.
#define CG_OpenGL // render OpenGL.
// #define CG_DirectX // render DirectX.
// #define CG_Privado // código do professor.

// Escolher qual numero rodar
#define CG_N2_1
// #define CG_N2_2
// #define CG_N2_3
// #define CG_N2_4
// #define CG_N2_5

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
// using OpenTK.Mathematics;

namespace gcgcg
{
    public class Mundo : GameWindow
    {
        private static Objeto mundo = null;

        private char rotuloAtual = '?';
        private Objeto objetoSelecionado = null;

        private readonly float[] _sruEixos =
        {
            -0.5f,  0.0f,  0.0f, /* X- */      0.5f,  0.0f,  0.0f, /* X+ */
            0.0f, -0.5f,  0.0f, /* Y- */      0.0f,  0.5f,  0.0f, /* Y+ */
            0.0f,  0.0f, -0.5f, /* Z- */      0.0f,  0.0f,  0.5f, /* Z+ */
        };

        private int _vertexBufferObject_sruEixos;
        private int _vertexArrayObject_sruEixos;

        private Shader _shaderVermelha;
        private Shader _shaderVerde;
        private Shader _shaderAzul;
        private Shader _shaderAmarela;
        private Shader _shaderMagenta;

        private bool mouseMovtoPrimeiro = true;
        private Ponto4D mouseMovtoUltimo;

        public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
               : base(gameWindowSettings, nativeWindowSettings)
        {
            mundo ??= new Objeto(null, ref rotuloAtual); //padrão Singleton
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            #region Eixos: SRU  
            _vertexBufferObject_sruEixos = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_sruEixos);
            GL.BufferData(BufferTarget.ArrayBuffer, _sruEixos.Length * sizeof(float), _sruEixos, BufferUsageHint.StaticDraw);
            _vertexArrayObject_sruEixos = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject_sruEixos);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
            _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
            _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
            _shaderAmarela = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
            _shaderMagenta = new Shader("Shaders/shader.vert", "Shaders/shaderMagenta.frag");
            #endregion

            #if CG_N2_1
            objetoSelecionado = new Circulo(mundo, ref rotuloAtual, 0.5);
            objetoSelecionado.ShaderObjeto = _shaderAmarela;
            #endif

            #if CG_N2_2
            objetoSelecionado = new Retangulo(mundo, ref rotuloAtual, new Ponto4D(-0.50, -0.50), new Ponto4D(0.50, 0.50));
            objetoSelecionado.ShaderObjeto = _shaderMagenta;
            objetoSelecionado.PrimitivaTipo = PrimitiveType.Points;
            #endif

            #if CG_N2_3
            objetoSelecionado = new SrPalito(mundo, ref rotuloAtual);
            #endif

            #if CG_N2_4
            objetoSelecionado = new Spline(mundo, ref rotuloAtual);
            #endif

            #if CG_N2_5
            objetoSelecionado = new Circulo(mundo, ref rotuloAtual, 0.3, new Ponto4D(0.3, 0.3));
            objetoSelecionado.PrimitivaTipo = PrimitiveType.LineLoop;
            objetoSelecionado.PrimitivaTamanho = 5;

            objetoSelecionado = new Retangulo(mundo, ref rotuloAtual, new Ponto4D(0.089, 0.089), new Ponto4D(0.511, 0.511));
            objetoSelecionado.PrimitivaTipo = PrimitiveType.LineLoop;

            var ponto = new Ponto4D(0.3, 0.3);

            objetoSelecionado = new Circulo(mundo, ref rotuloAtual, 0.1, ponto);
            objetoSelecionado.PrimitivaTipo = PrimitiveType.LineLoop;
            objetoSelecionado.PrimitivaTamanho = 5;

            objetoSelecionado = new Ponto(mundo, ref rotuloAtual, ponto);
            objetoSelecionado.PrimitivaTamanho = 10;
            #endif

#if CG_Privado
            #region Objeto: circulo - origem
            objetoSelecionado = new Circulo(mundo, ref rotuloAtual, 0.2)
            {
                ShaderObjeto = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag")
            };
            #endregion
            #region Objeto: circulo
            objetoSelecionado = new Circulo(mundo, ref rotuloAtual, 0.1, new Ponto4D(0.0, -0.5))
            {
                ShaderObjeto = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag")
            };
            #endregion

            #region Objeto: SrPalito  
            objetoSelecionado = new SrPalito(mundo, ref rotuloAtual);
            #endregion

            #region Objeto: SplineBezier
            objetoSelecionado = new SplineBezier(mundo, ref rotuloAtual);
            #endregion

            #region Objeto: SplineInter
            objetoSelecionado = new SplineInter(mundo, ref rotuloAtual);
            #endregion
#endif

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

#if CG_Gizmo
            Sru3D();
#endif
            mundo.Desenhar();
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            #region Teclado
            var input = KeyboardState;
            if (input.IsKeyPressed(Keys.Escape))
            {
                Close();
            }

            #if CG_N2_2
            if (input.IsKeyPressed(Keys.Space))
            {
                var tipo = (int)objetoSelecionado.PrimitivaTipo;
                tipo++;

                if (tipo >= 7)
                    tipo = 0;

                var primitivaTipo = (PrimitiveType)tipo;
                objetoSelecionado.PrimitivaTipo = primitivaTipo;
            }
            #endif

            #if CG_N2_3
            if (input.IsKeyPressed(Keys.Q))
            {
                var srPalito = (SrPalito)objetoSelecionado;
                srPalito.AtualizarPe(-0.05);
            }
            if (input.IsKeyPressed(Keys.W))
            {
                var srPalito = (SrPalito)objetoSelecionado;
                srPalito.AtualizarPe(0.05);
            }
            if (input.IsKeyPressed(Keys.A))
            {
                var srPalito = (SrPalito)objetoSelecionado;
                srPalito.AtualizarRaio(-0.05);
            }
            if (input.IsKeyPressed(Keys.S))
            {
                var srPalito = (SrPalito)objetoSelecionado;
                srPalito.AtualizarRaio(0.05);
            }
            if (input.IsKeyPressed(Keys.Z))
            {
                var srPalito = (SrPalito)objetoSelecionado;
                srPalito.AtualizarAngulo(-5);
            }
            if (input.IsKeyPressed(Keys.X))
            {
                var srPalito = (SrPalito)objetoSelecionado;
                srPalito.AtualizarAngulo(5);
            }
            #endif

            #if CG_N2_4
            if (input.IsKeyPressed(Keys.Space))
            {
                var spline = (Spline)objetoSelecionado;
                spline.AtualizarSpline(new Ponto4D(0.0, 0.0), true);
            }
            if (input.IsKeyPressed(Keys.C))
            {
                var spline = (Spline)objetoSelecionado;
                spline.AtualizarSpline(new Ponto4D(0.0, 0.1), false);
            }
            if (input.IsKeyPressed(Keys.B))
            {
                var spline = (Spline)objetoSelecionado;
                spline.AtualizarSpline(new Ponto4D(0.0, -0.1), false);
            }
            if (input.IsKeyPressed(Keys.E))
            {
                var spline = (Spline)objetoSelecionado;
                spline.AtualizarSpline(new Ponto4D(-0.1, 0.0), false);
            }
            if (input.IsKeyPressed(Keys.D))
            {
                var spline = (Spline)objetoSelecionado;
                spline.AtualizarSpline(new Ponto4D(0.1, 0.0), false);
            }
            if (input.IsKeyPressed(Keys.Equal))
            {
                var spline = (Spline)objetoSelecionado;
                spline.SplineQtdPto(1);
            }
            if (input.IsKeyPressed(Keys.Minus))
            {
                var spline = (Spline)objetoSelecionado;
                spline.SplineQtdPto(-1);
            }
            #endif

            #if CG_N2_5
            if (input.IsKeyPressed(Keys.C))
            {
                this.AtualizarJoystickVirtual(new Ponto4D(0.0, 0.02));
            }
            if (input.IsKeyPressed(Keys.B))
            {
                this.AtualizarJoystickVirtual(new Ponto4D(0.0, -0.02));
            }
            if (input.IsKeyPressed(Keys.E))
            {
                this.AtualizarJoystickVirtual(new Ponto4D(-0.02, 0.0));
            }
            if (input.IsKeyPressed(Keys.D))
            {
                this.AtualizarJoystickVirtual(new Ponto4D(0.02, 0.0));
            }
            #endif

            #endregion

            #region  Mouse
            int janelaLargura = Size.X;
            int janelaAltura = Size.Y;
            Ponto4D mousePonto = new Ponto4D(MousePosition.X, MousePosition.Y);
            Ponto4D sruPonto = Utilitario.NDC_TelaSRU(janelaLargura, janelaAltura, mousePonto);

            //FIXME: o movimento do mouse em relação ao eixo X está certo. Mas tem um erro no eixo Y,,, aumentar o valor do Y aumenta o erro.
            if (input.IsKeyDown(Keys.LeftShift))
            {
                if (mouseMovtoPrimeiro)
                {
                    mouseMovtoUltimo = sruPonto;
                    mouseMovtoPrimeiro = false;
                }
                else
                {
                    var deltaX = sruPonto.X - mouseMovtoUltimo.X;
                    var deltaY = sruPonto.Y - mouseMovtoUltimo.Y;
                    mouseMovtoUltimo = sruPonto;

                    objetoSelecionado.PontosAlterar(new Ponto4D(objetoSelecionado.PontosId(0).X + deltaX, objetoSelecionado.PontosId(0).Y + deltaY, 0), 0);
                    objetoSelecionado.ObjetoAtualizar();
                }
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                objetoSelecionado.PontosAlterar(sruPonto, 0);
                objetoSelecionado.ObjetoAtualizar();
            }
            #endregion

        }

        #if CG_N2_5
        private void AtualizarJoystickVirtual(Ponto4D pontoInc)
        {
            objetoSelecionado = mundo;
            objetoSelecionado = mundo.GrafocenaBuscaProximo(objetoSelecionado);
            var circuloMaior = (Circulo)objetoSelecionado;
            objetoSelecionado = mundo.GrafocenaBuscaProximo(objetoSelecionado);
            var retangulo = (Retangulo)objetoSelecionado;
            objetoSelecionado = mundo.GrafocenaBuscaProximo(objetoSelecionado);
            var circuloMenor = (Circulo)objetoSelecionado;
            objetoSelecionado = mundo.GrafocenaBuscaProximo(objetoSelecionado);
            var ponto = (Ponto)objetoSelecionado;

            var ponto4D = ponto.PontosId(0);
            var pontoFuturo = ponto4D + pontoInc;
            var ptoInfEsq = retangulo.PontosId(0);
            var ptoSupDir = retangulo.PontosId(2);

            if (PontoEstaForaBBoxInterna(pontoFuturo, ptoInfEsq, ptoSupDir))
            {
                retangulo.PrimitivaTipo = PrimitiveType.Points;

                if (PontoEstaForaCirculoMaior(pontoFuturo, circuloMaior))
                    return;
            }
            else
            {
                retangulo.PrimitivaTipo = PrimitiveType.LineLoop;
            }

            ponto4D.X += pontoInc.X;
            ponto4D.Y += pontoInc.Y;

            circuloMenor.Atualizar();
            ponto.Atualizar();
        }

        private bool PontoEstaForaBBoxInterna(Ponto4D ponto, Ponto4D pontoInfEsq, Ponto4D pontoSupDir)
        {
            var maxY = pontoSupDir.Y;
            var minY = pontoInfEsq.Y;
            var minX = pontoInfEsq.X;
            var maxX = pontoSupDir.X;

            return ponto.Y > maxY || // cima
                   ponto.Y < minY || // baixo
                   ponto.X < minX || // esquerda
                   ponto.X > maxX;   // direita
        }

        private bool PontoEstaForaCirculoMaior(Ponto4D ponto, Circulo circuloMaior)
        {
            return !(Matematica.Distancia(ponto, circuloMaior.PtoDeslocamento) <= circuloMaior.Raio);
        }
        #endif

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnUnload()
        {
            mundo.OnUnload();

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(_vertexBufferObject_sruEixos);
            GL.DeleteVertexArray(_vertexArrayObject_sruEixos);

            GL.DeleteProgram(_shaderVermelha.Handle);
            GL.DeleteProgram(_shaderVerde.Handle);
            GL.DeleteProgram(_shaderAzul.Handle);

            base.OnUnload();
        }

#if CG_Gizmo
        private void Sru3D()
        {
#if CG_OpenGL && !CG_DirectX
            GL.BindVertexArray(_vertexArrayObject_sruEixos);
            // EixoX
            _shaderVermelha.Use();
            GL.DrawArrays(PrimitiveType.Lines, 0, 2);
            // EixoY
            _shaderVerde.Use();
            GL.DrawArrays(PrimitiveType.Lines, 2, 2);
            // EixoZ
            _shaderAzul.Use();
            GL.DrawArrays(PrimitiveType.Lines, 4, 2);
#elif CG_DirectX && !CG_OpenGL
      Console.WriteLine(" .. Coloque aqui o seu código em DirectX");
#elif (CG_DirectX && CG_OpenGL) || (!CG_DirectX && !CG_OpenGL)
      Console.WriteLine(" .. ERRO de Render - escolha OpenGL ou DirectX !!");
#endif
        }
#endif

    }
}
