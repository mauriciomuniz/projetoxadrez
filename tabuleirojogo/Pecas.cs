namespace tabuleirojogo
{
    abstract class Pecas
    {
        public Posicao posicao { get; set; }
        public Cores cor { get; protected set; }
        public int QuantidadeMovimento { get; protected set; }
        public TabuleiroXadrez tab { get; protected set; }

        public Pecas(TabuleiroXadrez tab, Cores cor)
        {
            this.posicao = null;
            this.tab = tab;
            this.cor = cor;
            QuantidadeMovimento = 0;
        }

        public void IncrementarMovimentos()
        {
            QuantidadeMovimento++;
        }

        public void DecrementarQteMovimentos()
        {
            QuantidadeMovimento--;
        }

        public bool ExisteMovimentosPossiveis()
        {
            bool[,] mat = MovimentosPossiveis();
            for (int i = 0; i < tab.linha; i++)
            {
                for(int j = 0; j < tab.coluna; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool MovimentoPossivel(Posicao pos)
        {
            return MovimentosPossiveis()[pos.Linha, pos.Coluna];
        }

        public abstract bool[,] MovimentosPossiveis();
    }
}