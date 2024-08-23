using MySql.Data.MySqlClient;

namespace appLivros
{
    class Program
    {
        static string connectionString = "server=localhost;database=biblioteca;user=Daniel;password=1234567;";

        static void Main(string[] args)
        {
            int opcao;

            do
            {
                Console.WriteLine("\nSistema de Gerenciamento de Livros");
                Console.WriteLine("1. Adicionar Livro");
                Console.WriteLine("2. Listar Livros");
                Console.WriteLine("3. Editar Livro");
                Console.WriteLine("4. Excluir Livro");
                Console.WriteLine("5. Sair");
                Console.Write("Escolha uma opção: ");
                opcao = int.Parse(Console.ReadLine());

                switch (opcao)
                {
                    case 1:
                        AdicionarLivro();
                        break;
                    case 2:
                        ListarLivros();
                        break;
                    case 3:
                        EditarLivro();
                        break;
                    case 4:
                        ExcluirLivro();
                        break;
                    case 5:
                        Console.WriteLine("Saindo...");
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            } while (opcao != 5);
        }

        static void AdicionarLivro()
        {
            Console.Write("Título: ");
            string titulo = Console.ReadLine();

            if (TituloExiste(titulo))
            {
                Console.WriteLine("Erro: Um livro com esse título já existe.");
                return;
            }

            Console.Write("Autor: ");
            string autor = Console.ReadLine();

            Console.Write("Ano de Publicação: ");
            int anoPublicacao = int.Parse(Console.ReadLine());

            Console.Write("Gênero: ");
            string genero = Console.ReadLine();

            Console.Write("Número de Páginas: ");
            int numeroPaginas = int.Parse(Console.ReadLine());

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO livros (Titulo, Autor, AnoPublicacao, Genero, NumeroPaginas) VALUES (@Titulo, @Autor, @AnoPublicacao, @Genero, @NumeroPaginas)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Titulo", titulo);
                cmd.Parameters.AddWithValue("@Autor", autor);
                cmd.Parameters.AddWithValue("@AnoPublicacao", anoPublicacao);
                cmd.Parameters.AddWithValue("@Genero", genero);
                cmd.Parameters.AddWithValue("@NumeroPaginas", numeroPaginas);

                cmd.ExecuteNonQuery();
                Console.WriteLine("Livro adicionado com sucesso!");
            }
        }

        static bool TituloExiste(string titulo)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM livros WHERE Titulo = @Titulo";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Titulo", titulo);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        static void ListarLivros()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM livros";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\nLista de Livros:");
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["Id"]} | Título: {reader["Titulo"]} | Autor: {reader["Autor"]} | Ano: {reader["AnoPublicacao"]} | Gênero: {reader["Genero"]} | Páginas: {reader["NumeroPaginas"]}");
                }
            }
        }

        static void EditarLivro()
        {
            Console.Write("Informe o ID do livro que deseja editar: ");
            int id = int.Parse(Console.ReadLine());

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM livros WHERE Id = @Id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Console.Write("Novo Título: ");
                    string novoTitulo = Console.ReadLine();
                

 ,                  if (TituloExiste(novoTitulo) && novoTitulo != reader["Titulo"].ToString())
                    {
                        Console.WriteLine("Erro: Um livro com esse título já existe.");
                        return;
                    }

                    Console.Write("Novo Autor: ");
                    string novoAutor = Console.ReadLine();

                    Console.Write("Novo Ano de Publicação: ");
                    int novoAnoPublicacao = int.Parse(Console.ReadLine());

                    Console.Write("Novo Gênero: ");
                    string novoGenero = Console.ReadLine();

                    Console.Write("Novo Número de Páginas: ");
                    int novoNumeroPaginas = int.Parse(Console.ReadLine());

                    reader.Close();

                    string updateQuery = "UPDATE livros SET Titulo = @NovoTitulo, Autor = @NovoAutor, AnoPublicacao = @NovoAnoPublicacao, Genero = @NovoGenero, NumeroPaginas = @NovoNumeroPaginas WHERE Id = @Id";
                    cmd = new MySqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@NovoTitulo", novoTitulo);
                    cmd.Parameters.AddWithValue("@NovoAutor", novoAutor);
                    cmd.Parameters.AddWithValue("@NovoAnoPublicacao", novoAnoPublicacao);
                    cmd.Parameters.AddWithValue("@NovoGenero", novoGenero);
                    cmd.Parameters.AddWithValue("@NovoNumeroPaginas", novoNumeroPaginas);
                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Livro atualizado com sucesso!");
                }
                else
                {
                    Console.WriteLine("Livro não encontrado.");
                }
            }
        }

        static void ExcluirLivro()
        {
            Console.Write("Informe o ID do livro que deseja excluir: ");
            int id = int.Parse(Console.ReadLine());

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM livros WHERE Id = @Id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Livro excluído com sucesso!");
                }
                else
                {
                    Console.WriteLine("Livro não encontrado.");
                }
            }
        }
    }
}