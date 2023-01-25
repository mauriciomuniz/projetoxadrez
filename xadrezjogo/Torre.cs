using tabuleirojogo;

namespace xadrezjogo
{
    class Torre : Pecas
    {
        public Torre(TabuleiroXadrez tab, Cores cor) : base(tab, cor)
        {
        }

        public override string ToString()
        {
            return "T";
        }

        private bool podeMover(Posicao pos)
        {
            Pecas p = tab.PosicaoPeca(pos);
            return p == null || p.cor != cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[tab.linha, tab.coluna];
            Posicao pos = new Posicao(0, 0);

            //Acima
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna );
            while (tab.PosicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.PosicaoPeca(pos) != null && tab.PosicaoPeca(pos).cor != cor)
                {
                    break;
                }
                pos.Linha = pos.Linha - 1;
            }

            //Abaixo
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna);
            while (tab.PosicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.PosicaoPeca(pos) != null && tab.PosicaoPeca(pos).cor != cor)
                {
                    break;
                }
                pos.Linha = pos.Linha + 1;
            }

            //Direita
            pos.DefinirValores(posicao.Linha, posicao.Coluna + 1);
            while (tab.PosicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.PosicaoPeca(pos) != null && tab.PosicaoPeca(pos).cor != cor)
                {
                    break;
                }
                pos.Coluna = pos.Coluna + 1;
            }

            //Esquerda
            pos.DefinirValores(posicao.Linha, posicao.Coluna - 1);
            while (tab.PosicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.PosicaoPeca(pos) != null && tab.PosicaoPeca(pos).cor != cor)
                {
                    break;
                }
                pos.Coluna = pos.Coluna - 1;
            }

            return mat;
        }
    }
}