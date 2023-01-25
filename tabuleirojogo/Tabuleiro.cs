namespace tabuleirojogo
{
    class TabuleiroXadrez
    {
        
        public int coluna { get; set; }
        public int linha { get; set; }
        private Pecas[,] _pecas;

        public TabuleiroXadrez(int linha, int coluna)
        {
            this.linha = linha;
            this.coluna = coluna;
            _pecas = new Pecas[linha, coluna];
        }

        public Pecas PosicaoPeca(Posicao pos)
        {
            return _pecas[pos.Linha, pos.Coluna];
        }

        public Pecas RetornoPecas(int linha, int coluna)
        {
            return _pecas[linha, coluna];
        }

        public void BotarPeca(Posicao pos, Pecas p)
        {
            if (ExistePeca(pos))
            {
                throw new TabuleiroException("Nessa posição já existe uma peça.");
            }
            _pecas[pos.Linha, pos.Coluna] = p;
            p.posicao = pos;
        }

        public Pecas RemovePeca(Posicao pos)
        {
            if (PosicaoPeca(pos) == null)
            {
                return null;
            }
            Pecas aux = PosicaoPeca(pos);
            aux.posicao = null;
            _pecas[pos.Linha, pos.Coluna] = null;
            return aux;
        }

        public bool PosicaoValida(Posicao pos)
        {
            if (pos.Linha < 0 || pos.Linha >= linha || pos.Coluna < 0 || pos.Coluna >= coluna)
            {
                return false;
            }
            return true;
        }

        public bool ExistePeca(Posicao pos)
        {
            ValidarPosicao(pos);
            return PosicaoPeca(pos) != null;
        }

        public void ValidarPosicao(Posicao pos)
        {
            if (!PosicaoValida(pos))
            {
                throw new TabuleiroException("Posição Invalida!");
            }
        }
    }
}