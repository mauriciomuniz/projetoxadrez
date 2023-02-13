using System;
using tabuleirojogo;
using xadrezjogo;
using user;
using System.Collections.Generic;
using System.Text.Json;

namespace projetoxadrez
{
    class Tela
    {
        public static void ImprimirPartida(PartidaXadrez partida)
        {
            ImprimirTabuleiro(partida.tab);
            Console.WriteLine();
            ImprimirPecasCapturadas(partida);
            Console.WriteLine();
            Console.WriteLine("Turno: " + partida.turno);
            if (!partida.terminada)
            {
                Console.WriteLine("Aguardando jogada de: " +  partida.UsuarioLogado.Name + " da cor: " + partida.jogadorAtual);
                if (partida.xeque)
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("XEQUE!");
                    Console.ForegroundColor = aux;
                }
            }
            else
            {
                ConsoleColor aux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("XEQUEMATE!");
                Console.ForegroundColor = aux;


                Console.WriteLine((partida.UsuarioLogado.ContadorVitorias));
                Console.WriteLine("Vencedor: " + partida.jogadorAtual +" Usuário: "+ partida.UsuarioLogado.Name);
                
            }
        }

        

        public static void ImprimirPecasCapturadas(PartidaXadrez partida)
        {
            Console.WriteLine("Peças capturadas: ");
            Console.Write("Brancas: ");
            ImprimirConjunto(partida.PecasCapturadas(Cores.CorBranca));
            Console.WriteLine();
            Console.Write("Pretas: ");
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            ImprimirConjunto(partida.PecasCapturadas(Cores.CorPreta));
            Console.ForegroundColor = aux;
            Console.WriteLine();
        }

        public static void ImprimirConjunto(HashSet<Pecas> conjunto)
        {
            Console.Write("[");
            foreach (Pecas x in conjunto)
            {
                Console.Write(x + " ");
            }
            Console.Write("]");
        }

        public static void ImprimirTabuleiro(TabuleiroXadrez tab)
        {
            for (int i = 0; i < tab.linha; i++)
            {
                ConsoleColor aux1 = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(8 - i + " ");
                Console.ForegroundColor = aux1;

                for (int j = 0; j < tab.coluna; j++)
                { 
                        ImprimirPecas(tab.RetornoPecas(i, j));
                }
                Console.WriteLine();
            }
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  A B C D E F G H");
            Console.ForegroundColor = aux;
        }

        public static void ImprimirTabuleiro(TabuleiroXadrez tab, bool[,] posicoesPossiveis)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;

            for (int i = 0; i < tab.linha; i++)
            {
                ConsoleColor aux1 = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(8 - i + " ");
                Console.ForegroundColor = aux1;

                for (int j = 0; j < tab.coluna; j++)
                {
                    if(posicoesPossiveis[i, j])
                    {
                        Console.BackgroundColor = fundoAlterado;
                    }
                    else
                    {
                        Console.BackgroundColor = fundoOriginal;
                    }

                    ImprimirPecas(tab.RetornoPecas(i, j));
                    Console.BackgroundColor = fundoOriginal;
                }
                Console.WriteLine();
            }
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  A B C D E F G H");
            Console.ForegroundColor = aux;
            Console.BackgroundColor = fundoOriginal;
        }

        public static PosicaoXadrez LerPosicaoXadrez()
        {
            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse(s[1] + "");
            return new PosicaoXadrez(coluna, linha);
        }

        public static void ImprimirPecas(Pecas peca)
        {
            if (peca == null)
            {
                Console.Write("- ");
            }
            else
            {
                if (peca.cor == Cores.CorBranca)
                {
                    Console.Write(peca);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }

        public static void TelaMenu(PartidaXadrez partida)
        {
            int opcao;
            do
            {
                Console.Clear();
                Console.WriteLine("1 - Login");
                Console.WriteLine("2 - Fazer Cadastro");
                Console.WriteLine("3 - Ranking");
                Console.WriteLine("4 - Todos usuários cadastrados");
                
                Console.Write("Escolha a opção desejada: ");
                opcao = int.Parse(Console.ReadLine());
                Console.Clear();
                switch (opcao)
                {

                    case 1:
                        while (!partida.Logado)
                        {
                            try
                            {
                                TelaLogin(partida);
                                Console.Clear();
                            }
                            catch (ErroPartida e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("Realize a operação corretamente");
                                Console.ReadKey();
                                Console.Clear();
                            }
                        }
                        break;
                    case 2:
                        try
                        {
                            TelaCadastro(partida);
                            Console.WriteLine("Pronto! cadastro realizado com sucesso.");
                            Console.WriteLine("Qualquer digito para voltar a tela anterior");
                            Console.ReadKey();
                        }
                        catch (ErroPartida e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("Digite qualquer tecla.");
                            Console.ReadKey();
                        }
                        break;
                    case 3:
                        Console.WriteLine("Ranking dos usuários: ");
                        TelaOrdenados(partida);
                        Console.ReadKey();
                        break;
                    
                    case 4:
                        TelaJogadores(partida);
                        Console.WriteLine("Qualquer digito para voltar a tela anterior");
                        Console.ReadKey();
                        break;
                        
                    default:
                        Console.WriteLine("Número inválido. Qualquer digito para voltar a tela anterior");
                        Console.ReadKey();
                        break;
                }
            } while (opcao < 1 || opcao > 4 || !partida.Logado);
        }

        public static void TelaCadastro(PartidaXadrez partida)
        {
            Console.WriteLine("Cadastro de Usuários");
            Console.Write("Digite o seu nome de usuário: ");
            string name = Console.ReadLine();
            Console.Write("Digite uma senha: ");
            string password = Console.ReadLine();
            Console.WriteLine();
            partida.FazCadastro(name,password);
            Console.WriteLine($"Cadastro feito");

        }

        public static void TelaLogin(PartidaXadrez partida)
        {
            Console.Write($"Digite o nome de usuário: ");
            string name = Console.ReadLine();
            Console.Write("Digite a sua senha: ");
            string password = Console.ReadLine();
            bool usuarioLogado = partida.FazLogin(name, password);
            if (!usuarioLogado)
            {
                throw new ErroPartida("Usuário ou senha inválidos.");
            }
            Console.WriteLine("Login efetuado com sucesso!");
            Console.ReadKey();
            Console.Clear();
            if (partida.Usuario1 != null && partida.Usuario2 != null)
            {
                Console.Write("Olá " + partida.Usuario1.Name + " e ");
                Console.WriteLine(partida.Usuario2.Name + " hora do jogo!");
                Console.WriteLine("Aperte qualquer tecla");
                Console.ReadKey();
            }

        }

        public static void TelaJogadores(PartidaXadrez partida)
        {
            Console.WriteLine("Jogadores cadastrados: ");
            foreach (Usuario u in partida.Usuarios)
            {
                Console.WriteLine($"Usuário: {u.Name} - Partidas ganhas: {u.ContadorVitorias}");
                Console.WriteLine();
            }
        }

        public static void TelaOrdenados(PartidaXadrez partida)
        {
            int i=1;
            List<Usuario> UsuariosOrdenados = partida.Usuarios.OrderByDescending(u=>u.ContadorVitorias).ToList();
            // Console.WriteLine(String.Join(Environment.NewLine,UsuariosOrdenados));
            foreach(Usuario u in UsuariosOrdenados)
            {
                Console.WriteLine($"{i} - {u.Name} - Partidas ganhas: {u.ContadorVitorias}");
                i=i+1;
            }
        }








    }
}