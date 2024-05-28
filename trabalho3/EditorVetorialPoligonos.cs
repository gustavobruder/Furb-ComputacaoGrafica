#define CG_Debug

using System.Collections.Generic;
using System.Linq;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
    internal class EditorVetorialPoligonos : Objeto
    {
        private List<Poligono> _poligonos;
        private int _indicePoligonoSelecionado;
        private bool _estaEditandoPoligono;

        public EditorVetorialPoligonos(Objeto _paiRef, ref char _rotulo) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.Points;
            PrimitivaTamanho = 1;

            _poligonos = new List<Poligono>();
            _indicePoligonoSelecionado = -1;
            _estaEditandoPoligono = false;

            Atualizar();
        }

        public bool EstaEditandoPoligono()
        {
            return _estaEditandoPoligono;
        }

        public void AdicionarNovoPoligono(Poligono poligono)
        {
            if (EstaEditandoPoligono())
                return;

            _poligonos.Add(poligono);
            _indicePoligonoSelecionado = _poligonos.Count - 1;
            _estaEditandoPoligono = true;

            base.FilhoAdicionar(poligono);
            Atualizar();
        }

        public void AdicionarPontoPoligono(Ponto4D ponto)
        {
            if (!EstaEditandoPoligono())
                return;

            _poligonos.Last().PontosAdicionar(ponto);
            Atualizar();
        }

        public void FinalizarPoligono()
        {
            if (!EstaEditandoPoligono())
                return;

            _estaEditandoPoligono = false;

            Atualizar();
        }

        public Poligono ObterPoligonoSelecionado()
        {
            if (_indicePoligonoSelecionado < 0 || _indicePoligonoSelecionado >= _poligonos.Count)
                return null;

            return _poligonos[_indicePoligonoSelecionado];
        }

        public Poligono SelecionarPoligono(Ponto4D pontoClique)
        {
            for (var indicePoligono = 0; indicePoligono < _poligonos.Count; indicePoligono++)
            {
                var poligono = _poligonos[indicePoligono];

                var flagBBoxPoligono = poligono.Bbox().Dentro(pontoClique);

                if (!flagBBoxPoligono)
                    continue;

                var pontosPoligono = poligono.pontosLista.Append(poligono.pontosLista.First()).ToArray();
                var count = 0;

                for (var indicePonto = 0; indicePonto < poligono.pontosLista.Count; indicePonto++)
                {
                    var p1 = pontosPoligono[indicePonto];
                    var p2 = pontosPoligono[indicePonto + 1];

                    var flagScanLine = Matematica.ScanLine(pontoClique, p1, p2);

                    if (flagScanLine)
                        count++;
                }

                if (count % 2 != 0)
                {
                    _indicePoligonoSelecionado = indicePoligono;
                    return poligono;
                }
            }

            _indicePoligonoSelecionado = -1;
            return null;
        }

        public void AlterarPontoMaixProximoPoligonoSelecionado(Ponto4D pontoClique)
        {
            var poligonoSelecionado = ObterPoligonoSelecionado();

            if (poligonoSelecionado == null || poligonoSelecionado.pontosLista.Count == 0)
                return;

            (double Distancia, int Indice) pontoMenorDistancia = (double.MaxValue, -1);

            for (var i = 0; i < poligonoSelecionado.pontosLista.Count; i++)
            {
                var pontoPoligono = poligonoSelecionado.pontosLista[i];

                var distancia = Matematica.Distancia(pontoClique, pontoPoligono);

                if (distancia < pontoMenorDistancia.Distancia)
                    pontoMenorDistancia = (distancia, i);
            }

            poligonoSelecionado.PontosAlterar(pontoClique, pontoMenorDistancia.Indice);

            Atualizar();
        }

        public void RemoverPontoMaixProximoPoligonoSelecionado(Ponto4D pontoClique)
        {
            var poligonoSelecionado = ObterPoligonoSelecionado();

            if (poligonoSelecionado == null || poligonoSelecionado.pontosLista.Count == 0)
                return;

            (double Distancia, int Indice) pontoMenorDistancia = (double.MaxValue, -1);

            for (var i = 0; i < poligonoSelecionado.pontosLista.Count; i++)
            {
                var pontoPoligono = poligonoSelecionado.pontosLista[i];

                var distancia = Matematica.Distancia(pontoClique, pontoPoligono);

                if (distancia < pontoMenorDistancia.Distancia)
                    pontoMenorDistancia = (distancia, i);
            }

            poligonoSelecionado.PontosRemover(pontoMenorDistancia.Indice);

            Atualizar();
        }

        public void TranslacaoPoligonoSelecionado(double tx, double ty, double tz)
        {
            var poligonoSelecionado = ObterPoligonoSelecionado();

            if (poligonoSelecionado == null)
                return;

            poligonoSelecionado.MatrizTranslacaoXYZ(tx, ty, tz);

            Atualizar();
        }

        public void EscalaPoligonoSelecionado(double tx, double ty, double tz)
        {
            var poligonoSelecionado = ObterPoligonoSelecionado();

            if (poligonoSelecionado == null)
                return;

            var cloneCentroBBox = new Ponto4D(poligonoSelecionado.Bbox().ObterCentro);
            poligonoSelecionado.MatrizTranslacaoXYZ(-cloneCentroBBox.X, -cloneCentroBBox.Y, -cloneCentroBBox.Z);
            poligonoSelecionado.MatrizEscalaXYZ(tx, ty, tz);
            poligonoSelecionado.MatrizTranslacaoXYZ(cloneCentroBBox.X, cloneCentroBBox.Y, cloneCentroBBox.Z);

            Atualizar();
        }

        public void RotacaoPoligonoSelecionado(double angulo)
        {
            var poligonoSelecionado = ObterPoligonoSelecionado();

            if (poligonoSelecionado == null)
                return;

            var cloneCentroBBox = new Ponto4D(poligonoSelecionado.Bbox().ObterCentro);
            poligonoSelecionado.MatrizTranslacaoXYZ(-cloneCentroBBox.X, -cloneCentroBBox.Y, -cloneCentroBBox.Z);
            poligonoSelecionado.MatrizRotacao(angulo);
            poligonoSelecionado.MatrizTranslacaoXYZ(cloneCentroBBox.X, cloneCentroBBox.Y, cloneCentroBBox.Z);

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
        retorno = "__ Objeto EditorVetorialPoligonos _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
        retorno += base.ImprimeToString();
        return retorno;
    }
#endif

    }
}
