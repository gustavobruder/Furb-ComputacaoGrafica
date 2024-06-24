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
    private Ponto4D _centro;
    private double _tamanhoLado;
    private Ponto4D[] _vertices;
    // int[] indices;
    // Vector3[] normals;
    // int[] colors;

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

      var indices = new uint[]
      {
        // Frente
        3, 2, 6, // Triangulo superior
        6, 7, 3, // Triangulo inferior

        // Cima
        0, 1, 2, // Triangulo superior
        2, 3, 0, // Triangulo inferior

        // Fundo
        0, 1, 5, // Triangulo superior
        5, 4, 0, // Triangulo inferior

        // Baixo
        4, 5, 6, // Triangulo superior
        6, 7, 4, // Triangulo inferior

        // Esquerda
        3, 0, 4, // Triangulo superior
        4, 7, 3, // Triangulo inferior

        // Direita
        2, 1, 5, // Triangulo superior
        5, 6, 2, // Triangulo inferior
      };

      foreach (int indice in indices)
      {
        PontosAdicionar(_vertices[indice]);
      }

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
