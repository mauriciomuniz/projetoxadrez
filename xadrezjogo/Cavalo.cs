using tabuleirojogo;

namespace xadrezjogo
{

    class Cavalo : Pecas
    {

        public Cavalo(TabuleiroXadrez tab, Cores cor) : base(tab, cor)
        {
        }

        public override string ToString()
        {
            return "C";
        }

        private bool PodeMover(Posicao pos)
        {
            Pecas p = tab.PosicaoPeca(pos);
            return p == null || p.cor != cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[tab.linha, tab.coluna];

            Posicao pos = new Posicao(0, 0);

            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna - 2);
            if (tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(posicao.Linha - 2, posicao.Coluna - 1);
            if (tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(posicao.Linha - 2, posicao.Coluna + 1);
            if (tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna + 2);
            if (tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna + 2);
            if (tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(posicao.Linha + 2, posicao.Coluna + 1);
            if (tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(posicao.Linha + 2, posicao.Coluna - 1);
            if (tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna - 2);
            if (tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }

            return mat;
        }
    }
}