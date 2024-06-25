//https://github.com/mono/opentk/blob/main/Source/Examples/Shapes/Old/Cube.cs

#define CG_Debug
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;

namespace gcgcg
{
    internal class Cubo : Objeto
    {
        private Shader _shaderBranca = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
        private Shader _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
        private Shader _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
        private Shader _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
        private Shader _shaderCiano = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");
        private Shader _shaderMagenta = new Shader("Shaders/shader.vert", "Shaders/shaderMagenta.frag");

        private Ponto4D _centro;
        private double _tamanhoLado;
        private Ponto4D[] _vertices;
        private Face[] _faces;

        public Cubo(Objeto _paiRef, ref char _rotulo, Ponto4D centro, double tamanhoLado) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.TriangleFan;
            PrimitivaTamanho = 10;

            _centro = centro;
            _tamanhoLado = tamanhoLado;

            var metadeLado = _tamanhoLado / 2;
            var maxX = _centro.X + metadeLado;
            var minX = _centro.X - metadeLado;
            var maxY = _centro.Y + metadeLado;
            var minY = _centro.Y - metadeLado;
            var maxZ = _centro.Z + metadeLado;
            var minZ = _centro.Z - metadeLado;

            _vertices = new Ponto4D[]
            {
                new Ponto4D(minX, maxY, minZ), // Ponto 0
                new Ponto4D(maxX, maxY, minZ), // Ponto 1
                new Ponto4D(maxX, maxY, maxZ), // Ponto 2
                new Ponto4D(minX, maxY, maxZ), // Ponto 3
                new Ponto4D(minX, minY, minZ), // Ponto 4
                new Ponto4D(maxX, minY, minZ), // Ponto 5
                new Ponto4D(maxX, minY, maxZ), // Ponto 6
                new Ponto4D(minX, minY, maxZ), // Ponto 7
            };

            var faceFrente = new Face(this, ref _rotulo, new[]
            {
                _vertices[3], _vertices[2], _vertices[6],
                _vertices[6], _vertices[7], _vertices[3],
            });
            var faceCima = new Face(this, ref _rotulo, new[]
            {
                _vertices[0], _vertices[1], _vertices[2],
                _vertices[2], _vertices[3], _vertices[0],
            });
            var faceFundo = new Face(this, ref _rotulo, new[]
            {
                _vertices[0], _vertices[1], _vertices[5],
                _vertices[5], _vertices[4], _vertices[0],
            });
            var faceBaixo = new Face(this, ref _rotulo, new[]
            {
                _vertices[4], _vertices[5], _vertices[6],
                _vertices[6], _vertices[7], _vertices[4],
            });
            var faceEsquerda = new Face(this, ref _rotulo, new[]
            {
                _vertices[3], _vertices[0], _vertices[4],
                _vertices[4], _vertices[7], _vertices[3],
            });
            var faceDireita = new Face(this, ref _rotulo, new[]
            {
                _vertices[2], _vertices[1], _vertices[5],
                _vertices[5], _vertices[6], _vertices[2],
            });

            faceFrente.shaderCor = _shaderBranca;
            faceCima.shaderCor = _shaderVermelha;
            faceFundo.shaderCor = _shaderVerde;
            faceBaixo.shaderCor = _shaderAzul;
            faceEsquerda.shaderCor = _shaderCiano;
            faceDireita.shaderCor = _shaderMagenta;

            _faces = new[]
            {
                faceFrente,
                faceCima,
                faceFundo,
                faceBaixo,
                faceEsquerda,
                faceDireita,
            };

            Atualizar();
        }

        private void Atualizar()
        {
            base.ObjetoAtualizar();
        }

#if CG_Debug
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Cubo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
            retorno += base.ImprimeToString();
            return (retorno);
        }
#endif
    }
}