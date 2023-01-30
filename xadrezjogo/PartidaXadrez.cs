using System;
using System.Collections.Generic;
using tabuleirojogo;
using user;
using System.Text.Json;

namespace xadrezjogo
{
    class PartidaXadrez
    {
        public TabuleiroXadrez tab { get; private set; }
        public int turno { get; private set; }
        public Cores jogadorAtual { get; private set; }
        public bool terminada { get; set; }
        private HashSet<Pecas> pecas;
        private HashSet<Pecas> capturadas;
        public Pecas vulneravelEnPassant { get; private set; }
        public bool xeque { get; private set; }
        public Usuario UsuarioLogado { get; private set; }
        public bool Logado { get; private set; }
        public List<Usuario> Usuarios { get; private set; }
        public Usuario Usuario1 { get; private set; }
        public Usuario Usuario2 { get; private set; }
        public string fileName = "usuarios.json";



        public PartidaXadrez()
        {
            tab = new TabuleiroXadrez(8, 8);

            FileInfo fi = new FileInfo(fileName);
            string deseriJson = File.ReadAllText(fileName);
            if (fi.Length == 0)
            {
                Usuarios = new List<Usuario>();
            }
            else
            {
                Usuarios = JsonSerializer.Deserialize<List<Usuario>>(deseriJson)!;
            } 

            turno = 1;
            jogadorAtual = Cores.CorBranca;
            xeque = false;
            vulneravelEnPassant = null;
            terminada = false;
            pecas = new HashSet<Pecas>();
            capturadas = new HashSet<Pecas>();
            ColocarPeca();
        }



        public void FazCadastro(string name, string password)
        {
            if (Usuarios.Exists(x => x.Name == name))
            {
                throw new ErroPartida($"O usuário {name} já existe. Escolha outro nome de usuário.");
            }
            else
            {
                Usuario usuario = new Usuario(name, password);
                Usuarios.Add(usuario);
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(Usuarios, options);
                File.WriteAllText(fileName, jsonString);
            }

        }
        public bool FazLogin(string name, string senha)
        {
            bool logarConta = Usuarios.Exists(x => x.Name == name && x.Password == senha);

            if (logarConta)
            {
                if (Usuario1 == null)
                {
                    Usuario1 = Usuarios.Find(x => x.Name == name && x.Password == senha);
                    UsuarioLogado = Usuario1;
                }
                else
                {
                    if (Usuarios.Find(x => x.Name == name && x.Password == senha) == Usuario1)
                    {
                        throw new ErroPartida("Esse usuário já está logado.");
                    }
                    else
                    {
                        Usuario2 = Usuarios.Find(x => x.Name == name && x.Password == senha);
                        Logado = true;
                    }
                }
                return true;
            }
            return false;
        }

        
        public void AtualizarJson()
        {
            foreach (Usuario u in Usuarios)
                {
                    if (u == UsuarioLogado)
                    { 
                        u.NumeroVitorias();                          
                    }
                }
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(Usuarios, options);
            File.WriteAllText(fileName, jsonString);
        }



        public Pecas ExecutarMovimento(Posicao origem, Posicao destino)
        {
            Pecas p = tab.RemovePeca(origem);
            p.IncrementarMovimentos();
            Pecas pecaCapturada = tab.RemovePeca(destino);
            tab.BotarPeca(destino, p);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Pecas T = tab.RemovePeca(origemT);
                T.IncrementarMovimentos();
                tab.BotarPeca(destinoT, T);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Pecas T = tab.RemovePeca(origemT);
                T.IncrementarMovimentos();
                tab.BotarPeca(destinoT, T);
            }

            // #jogadaespecial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if (p.cor == Cores.CorBranca)
                    {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    pecaCapturada = tab.RemovePeca(posP);
                    capturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Pecas pecaCapturada)
        {
            Pecas p = tab.RemovePeca(destino);
            p.DecrementarQteMovimentos();
            if (pecaCapturada != null)
            {
                tab.BotarPeca(destino, pecaCapturada);
                capturadas.Remove(pecaCapturada);
            }
            tab.BotarPeca(origem, p);

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Pecas T = tab.RemovePeca(destinoT);
                T.DecrementarQteMovimentos();
                tab.BotarPeca(origemT,  T);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Pecas T = tab.RemovePeca(destinoT);
                T.DecrementarQteMovimentos();
                tab.BotarPeca(origemT, T);
            }

            // #jogadaespecial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == vulneravelEnPassant)
                {
                    Pecas peao = tab.RemovePeca(destino);
                    Posicao posP;
                    if (p.cor == Cores.CorBranca)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    tab.BotarPeca(posP, peao);
                }
            }

        }

        public void RealizarJogada(Posicao origem, Posicao destino)
        {
           Pecas pecaCapturada = ExecutarMovimento(origem, destino);
            if (EstaEmXeque(jogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Pecas p = tab.PosicaoPeca(destino);

            // #jogadaespecial promocao
            if (p is Peao)
            {
                if ((p.cor == Cores.CorBranca && destino.Linha == 0) || (p.cor == Cores.CorPreta && destino.Linha == 7))
                {
                    p = tab.RemovePeca(destino);
                    pecas.Remove(p);

                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine();
                    Console.WriteLine("#--- PROMOÇÃO! ---#");
                    Console.WriteLine("Opções de promoção:");
                    Console.WriteLine(" - Dama[D]\n - Torre[T]\n - Bispo[B]\n - Cavalo[C]");
                    Console.ForegroundColor = aux;
                    Console.Write("Digite o caractere da opção escolhida: ");
                    
                    char escolha = char.Parse(Console.ReadLine());
                    
                    switch (escolha)
                    {
                        //DAMA
                        case 'D':
                            Pecas dama = new Dama(tab, p.cor);
                            tab.BotarPeca(destino, dama);
                            pecas.Add(dama);
                            break;
                        case 'd':
                            Pecas dama1 = new Dama(tab, p.cor);
                            tab.BotarPeca(destino, dama1);
                            pecas.Add(dama1);
                            break;

                        //TORRE
                        case 'T':
                            Pecas torre = new Torre(tab, p.cor);
                            tab.BotarPeca(destino, torre);
                            pecas.Add(torre);
                            break;
                        case 't':
                            Pecas torre1 = new Torre(tab, p.cor);
                            tab.BotarPeca(destino, torre1);
                            pecas.Add(torre1);
                            break;

                        //BISPO
                        case 'B':
                            Pecas bispo = new Bispo(tab, p.cor);
                            tab.BotarPeca(destino, bispo);
                            pecas.Add(bispo);
                            break;
                        case 'b':
                            Pecas bispo1 = new Bispo(tab, p.cor);
                            tab.BotarPeca(destino, bispo1);
                            pecas.Add(bispo1);
                            break;

                        //CAVALO
                        case 'C':
                            Pecas cavalo = new Cavalo(tab, p.cor);
                            tab.BotarPeca(destino, cavalo);
                            pecas.Add(cavalo);
                            break;
                        case 'c':
                            Pecas cavalo1 = new Cavalo(tab, p.cor);
                            tab.BotarPeca(destino, cavalo1);
                            pecas.Add(cavalo1);
                            break;
                    }
                    
                }
            }

            if (EstaEmXeque(Adversario(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }
            
            //arrumar aqui
            if (TesteXequeMate(Adversario(jogadorAtual)))
            {   
                AtualizarJson();
                
                terminada = true;
            }
            else
            {
                turno++;
                MudaJogador();
            }

            
    
            // #jogadaespecial en passant
            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
            {
                vulneravelEnPassant = p;
            }
            else
            {
                vulneravelEnPassant = null;
            }

        }

        private Cores Adversario(Cores cor)
        {
            if (cor == Cores.CorBranca)
            {
                return Cores.CorPreta;
            }
            else
            {
                return Cores.CorBranca;
            }
        }

        private Pecas Rei(Cores cor)
        {
            foreach (Pecas x in PecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool EstaEmXeque(Cores cor)
        {
            Pecas R = Rei(cor);
            if (R == null)
            {
                throw new TabuleiroException("Não tem Rei da cor " + cor + " no tabuleiro!");
            }
            foreach (Pecas x in PecasEmJogo(Adversario(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[R.posicao.Linha, R.posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }


        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            if (tab.PosicaoPeca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if(jogadorAtual != tab.PosicaoPeca(pos).cor)
            {
                throw new TabuleiroException("A peça na posição de origem escolhida não é sua!");
            }
            if (!tab.PosicaoPeca(pos).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possiveis para a peça de origem escolhida!");
            }
        }

        public void ValidarPosicaoDeDestino( Posicao origem, Posicao destino)
        {
            if (!tab.PosicaoPeca(origem).MovimentoPossivel(destino))
            {
                throw new TabuleiroException("Posição de destino invalida!");
            }
        }

        private void MudaJogador()
        {
            if(jogadorAtual == Cores.CorBranca)
            {
                jogadorAtual = Cores.CorPreta;
                UsuarioLogado = Usuario2;
            }
            else
            {
                jogadorAtual = Cores.CorBranca;
                UsuarioLogado = Usuario1;
            }
        }

        public HashSet<Pecas> PecasCapturadas(Cores cor)
        {
            HashSet<Pecas> aux = new HashSet<Pecas>();
            foreach (Pecas x in capturadas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Pecas> PecasEmJogo(Cores cor)
        {
            HashSet<Pecas> aux = new HashSet<Pecas>();
            foreach(Pecas x in pecas)
            {
                if(x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        public bool TesteXequeMate(Cores cor)
        {
            if (!EstaEmXeque(cor))
            {
                return false;
            }
            foreach (Pecas x in PecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for (int i = 0; i < tab.linha; i++)
                {
                    for (int j = 0; j < tab.coluna; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.posicao;
                            Posicao destino = new Posicao(i, j);
                            Pecas pecaCapturada = ExecutarMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void ColocarNovaPeca(char coluna, int linha, Pecas peca)
        {
            tab.BotarPeca(new PosicaoXadrez(coluna, linha).toPosicao(), peca);
            pecas.Add(peca);
        }
        
        private void ColocarPeca()
        {
            ColocarNovaPeca('a', 1, new Torre(tab, Cores.CorBranca));
            ColocarNovaPeca('b', 1, new Cavalo(tab, Cores.CorBranca));
            ColocarNovaPeca('c', 1, new Bispo(tab, Cores.CorBranca));
            ColocarNovaPeca('d', 1, new Dama(tab, Cores.CorBranca));
            ColocarNovaPeca('e', 1, new Rei(tab, Cores.CorBranca, this));
            ColocarNovaPeca('f', 1, new Bispo(tab, Cores.CorBranca));
            ColocarNovaPeca('g', 1, new Cavalo(tab, Cores.CorBranca));
            ColocarNovaPeca('h', 1, new Torre(tab, Cores.CorBranca));
            ColocarNovaPeca('a', 2, new Peao(tab, Cores.CorBranca, this));
            ColocarNovaPeca('b', 2, new Peao(tab, Cores.CorBranca, this));
            ColocarNovaPeca('c', 2, new Peao(tab, Cores.CorBranca, this));
            ColocarNovaPeca('d', 2, new Peao(tab, Cores.CorBranca, this));
            ColocarNovaPeca('e', 2, new Peao(tab, Cores.CorBranca, this));
            ColocarNovaPeca('f', 2, new Peao(tab, Cores.CorBranca, this));
            ColocarNovaPeca('g', 2, new Peao(tab, Cores.CorBranca, this));
            ColocarNovaPeca('h', 2, new Peao(tab, Cores.CorBranca, this));

            //----------------------------------------------------\\
            
            ColocarNovaPeca('a', 8, new Torre(tab, Cores.CorPreta));
            ColocarNovaPeca('b', 8, new Cavalo(tab, Cores.CorPreta));
            ColocarNovaPeca('c', 8, new Bispo(tab, Cores.CorPreta));
            ColocarNovaPeca('d', 8, new Dama(tab, Cores.CorPreta));
            ColocarNovaPeca('e', 8, new Rei(tab, Cores.CorPreta, this));
            ColocarNovaPeca('f', 8, new Bispo(tab, Cores.CorPreta));
            ColocarNovaPeca('g', 8, new Cavalo(tab, Cores.CorPreta));
            ColocarNovaPeca('h', 8, new Torre(tab, Cores.CorPreta));
            ColocarNovaPeca('a', 7, new Peao(tab, Cores.CorPreta, this));
            ColocarNovaPeca('b', 7, new Peao(tab, Cores.CorPreta, this));
            ColocarNovaPeca('c', 7, new Peao(tab, Cores.CorPreta, this));
            ColocarNovaPeca('d', 7, new Peao(tab, Cores.CorPreta, this));
            ColocarNovaPeca('e', 7, new Peao(tab, Cores.CorPreta, this));
            ColocarNovaPeca('f', 7, new Peao(tab, Cores.CorPreta, this));
            ColocarNovaPeca('g', 7, new Peao(tab, Cores.CorPreta, this));
            ColocarNovaPeca('h', 7, new Peao(tab, Cores.CorPreta, this));
        }
    }
}