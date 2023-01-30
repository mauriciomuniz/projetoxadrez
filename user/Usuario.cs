using xadrezjogo;
using tabuleirojogo;

namespace user
{
    class Usuario
    {
        
        public string Name{get;set;}
        public string Password{get;set;}
        public int ContadorVitorias{get;set;}

        public Usuario(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public void NumeroVitorias()
        {
            ContadorVitorias++;
        }

        public override string ToString()
        {
            return $"usuario:{Name} partidas ganhas:{ContadorVitorias}";
        }
    }
}