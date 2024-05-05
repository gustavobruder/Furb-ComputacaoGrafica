#define CG_Debug

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
    internal class Circulo : Objeto
    {
        private const int AnguloInc = 360 / 72;

        private readonly double _raio;
        public Ponto4D PtoDeslocamento;

        public Circulo(Objeto _paiRef, ref char _rotulo, double _raio) : this(_paiRef, ref _rotulo, _raio, new Ponto4D())
        {
        }

        public Circulo(Objeto _paiRef, ref char _rotulo, double _raio, Ponto4D ptoDeslocamento) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.Points;
            PrimitivaTamanho = 5;

            this._raio = _raio;
            this.PtoDeslocamento = ptoDeslocamento;

            this.Atualizar();
        }

        public void Atualizar()
        {
            base.pontosLista.Clear();

            for (var angulo = 0; angulo < 360; angulo += AnguloInc)
            {
                var ponto4D = Matematica.GerarPtosCirculo(angulo, _raio);
                base.PontosAdicionar(ponto4D + PtoDeslocamento);
            }

            base.ObjetoAtualizar();
        }

#if CG_Debug
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Circulo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
            retorno += base.ImprimeToString();
            return (retorno);
        }
#endif
    }
}
