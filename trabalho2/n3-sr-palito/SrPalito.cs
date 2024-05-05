#define CG_Debug

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
    internal class SrPalito : Objeto
    {
        private double _raio;
        private int _angulo;

        private Ponto4D _pontoInicio;
        private Ponto4D _pontoFim;
        private SegReta _segReta;

        public SrPalito(Objeto _paiRef, ref char _rotulo) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.Lines;
            PrimitivaTamanho = 5;

            _raio = 0.5;
            _angulo = 45;

            _pontoInicio = new Ponto4D(0.0, 0.0);
            _pontoFim = Matematica.GerarPtosCirculo(_angulo, _raio);
            _segReta = new SegReta(_paiRef, ref _rotulo, _pontoInicio, _pontoFim);

            base.PontosAdicionar(_pontoInicio);
            base.PontosAdicionar(_pontoFim);

            this.Atualizar();
        }

        private void Atualizar()
        {
            base.PontosAlterar(_pontoInicio, 0);
            base.PontosAlterar(_pontoFim, 1);

            _segReta.PontosAlterar(_pontoInicio, 0);
            _segReta.PontosAlterar(_pontoFim, 1);

            base.ObjetoAtualizar();
        }

        public void AtualizarPe(double peInc)
        {
            _pontoInicio.X += peInc;
            _pontoFim.X += peInc;

            this.Atualizar();
        }

        public void AtualizarRaio(double raioInc)
        {
            _raio += raioInc;

            _pontoFim = Matematica.GerarPtosCirculo(_angulo, _raio);
            _pontoFim.X += _pontoInicio.X;

            this.Atualizar();
        }

        public void AtualizarAngulo(int anguloInc)
        {
            _angulo += anguloInc;

            _pontoFim = Matematica.GerarPtosCirculo(_angulo, _raio);
            _pontoFim.X += _pontoInicio.X;

            this.Atualizar();
        }

#if CG_Debug
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto SrPalito _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
            retorno += base.ImprimeToString();
            return (retorno);
        }
#endif
    }
}
