#define CG_Debug

using System.Collections.Generic;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
    internal class Spline : Objeto
    {
        private static readonly Shader _shaderAmarelo = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
        private static readonly Shader _shaderBranco = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
        private static readonly Shader _shaderCiano = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");
        private static readonly Shader _shaderVermelho = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");

        private Ponto4D _pontoControleInfDir;
        private Ponto4D _pontoControleSupDir;
        private Ponto4D _pontoControleSupEsq;
        private Ponto4D _pontoControleInfEsq;

        private SegReta _segRetaDir;
        private SegReta _segRetaSup;
        private SegReta _segRetaEsq;

        private List<Ponto> _pontosControle;
        private int _indicePontoControle;
        private int _qtdSegRetas;

        public Spline(Objeto _paiRef, ref char _rotulo) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.LineLoop;
            PrimitivaTamanho = 20;
            ShaderObjeto = _shaderAmarelo;

            _pontoControleInfDir = new Ponto4D(0.5, -0.5);
            _pontoControleSupDir = new Ponto4D(0.5, 0.5);
            _pontoControleSupEsq = new Ponto4D(-0.5, 0.5);
            _pontoControleInfEsq = new Ponto4D(-0.5, -0.5);

            _segRetaDir = new SegReta(_paiRef, ref _rotulo, _pontoControleInfDir, _pontoControleSupDir);
            _segRetaSup = new SegReta(_paiRef, ref _rotulo, _pontoControleSupDir, _pontoControleSupEsq);
            _segRetaEsq = new SegReta(_paiRef, ref _rotulo, _pontoControleSupEsq, _pontoControleInfEsq);

            _segRetaDir.ShaderObjeto = _shaderCiano;
            _segRetaSup.ShaderObjeto = _shaderCiano;
            _segRetaEsq.ShaderObjeto = _shaderCiano;

            _pontosControle =
            [
                new Ponto(_paiRef, ref _rotulo, _pontoControleInfDir),
                new Ponto(_paiRef, ref _rotulo, _pontoControleSupDir),
                new Ponto(_paiRef, ref _rotulo, _pontoControleSupEsq),
                new Ponto(_paiRef, ref _rotulo, _pontoControleInfEsq),
            ];

            _indicePontoControle = 0;
            _pontosControle[_indicePontoControle].ShaderObjeto = _shaderVermelho;
            _qtdSegRetas = 5;

            this.Atualizar();
        }

        private void Atualizar()
        {
            base.pontosLista.Clear();

            var tParcial = 1.0 / _qtdSegRetas;

            for (var i = 1; i < _qtdSegRetas; i++)
            {
                var t = tParcial * i;

                var p1p2 = CalcularPontoIntermediario(_pontoControleInfEsq, _pontoControleSupEsq, t);
                var p2p3 = CalcularPontoIntermediario(_pontoControleSupEsq, _pontoControleSupDir, t);
                var p3p4 = CalcularPontoIntermediario(_pontoControleSupDir, _pontoControleInfDir, t);

                var p1p2p3 = CalcularPontoIntermediario(p1p2, p2p3, t);
                var p2p3p4 = CalcularPontoIntermediario(p2p3, p3p4, t);

                var p1p2p3p4 = CalcularPontoIntermediario(p1p2p3, p2p3p4, t);
                base.PontosAdicionar(p1p2p3p4);
            }

            base.PontosAdicionar(_pontoControleInfDir);
            base.PontosAdicionar(_pontoControleSupDir);
            base.PontosAdicionar(_pontoControleSupEsq);
            base.PontosAdicionar(_pontoControleInfEsq);

            _segRetaDir.ObjetoAtualizar();
            _segRetaSup.ObjetoAtualizar();
            _segRetaEsq.ObjetoAtualizar();

            base.ObjetoAtualizar();
        }

        private Ponto4D CalcularPontoIntermediario(Ponto4D pontoA, Ponto4D pontoB, double t)
        {
            return new Ponto4D(
                pontoA.X + (pontoB.X - pontoA.X) * t,
                pontoA.Y + (pontoB.Y - pontoA.Y) * t
            );
        }

        public void SplineQtdPto(int inc)
        {
            // min
            if (_qtdSegRetas == 1 && inc < 0)
                return;

            // max
            if (_qtdSegRetas == 15 && inc > 0)
                return;

            _qtdSegRetas += inc;
            this.Atualizar();
        }

        public void AtualizarSpline(Ponto4D pontoInc, bool proximo)
        {
            if (proximo)
            {
                SelecionarProximoPonto();
                return;
            }

            AtualizarCoordenadasPontoSelecionado(pontoInc);
            this.Atualizar();
        }

        private void SelecionarProximoPonto()
        {
            _pontosControle[_indicePontoControle].ShaderObjeto = _shaderBranco;

            _indicePontoControle++;

            if (_indicePontoControle >= _pontosControle.Count)
                _indicePontoControle = 0;

            _pontosControle[_indicePontoControle].ShaderObjeto = _shaderVermelho;
        }

        private void AtualizarCoordenadasPontoSelecionado(Ponto4D pontoInc)
        {
            var pontoControle = _pontosControle[_indicePontoControle];
            var ponto = pontoControle.PontosId(0);
            ponto.X += pontoInc.X;
            ponto.Y += pontoInc.Y;
            pontoControle.PontosAlterar(ponto, 0);
        }

#if CG_Debug
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Spline _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
            retorno += base.ImprimeToString();
            return (retorno);
        }
#endif
    }
}
