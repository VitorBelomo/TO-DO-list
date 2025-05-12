using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class TaskItem
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public bool Concluida { get; set; }
}

class Program
{
    static string filePath = "tasks.json";
    static List<TaskItem> tarefas = new();

    static void Main()
    {
        CarregarTarefas();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Gerenciador de Tarefas ===");
            Console.WriteLine("1. Adicionar Tarefa");
            Console.WriteLine("2. Listar Tarefas");
            Console.WriteLine("3. Marcar Tarefa como Concluída");
            Console.WriteLine("4. Excluir Tarefa");
            Console.WriteLine("5. Sair");
            Console.Write("Escolha uma opção: ");
            var opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1": AdicionarTarefa(); break;
                case "2": ListarTarefas(); break;
                case "3": ConcluirTarefa(); break;
                case "4": ExcluirTarefa(); break;
                case "5": return;
                default: Console.WriteLine("Opção inválida."); break;
            }

            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }

    static void AdicionarTarefa()
    {
        Console.Write("Título da tarefa: ");
        string titulo = Console.ReadLine();
        Console.Write("Descrição: ");
        string descricao = Console.ReadLine();

        int novoId = tarefas.Count > 0 ? tarefas[^1].Id + 1 : 1;

        tarefas.Add(new TaskItem
        {
            Id = novoId,
            Titulo = titulo,
            Descricao = descricao,
            Concluida = false
        });

        SalvarTarefas();
        Console.WriteLine("Tarefa adicionada com sucesso.");
    }

    static void ListarTarefas()
    {
        Console.WriteLine("\n--- Tarefas ---");
        if (tarefas.Count == 0)
        {
            Console.WriteLine("Nenhuma tarefa encontrada.");
            return;
        }

        foreach (var t in tarefas)
        {
            Console.WriteLine($"{t.Id}. [{(t.Concluida ? "X" : " ")}] {t.Titulo} - {t.Descricao}");
        }
    }

    static void ConcluirTarefa()
    {
        Console.Write("ID da tarefa a marcar como concluída: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var tarefa = tarefas.Find(t => t.Id == id);
            if (tarefa != null)
            {
                tarefa.Concluida = true;
                SalvarTarefas();
                Console.WriteLine("Tarefa marcada como concluída.");
            }
            else
            {
                Console.WriteLine("Tarefa não encontrada.");
            }
        }
    }

    static void ExcluirTarefa()
    {
        ListarTarefas();
        Console.Write("Escolha uma tarefa para excluir: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var tarefa = tarefas.Find(t => t.Id == id);
            if (tarefa != null)
            {
                tarefas.Remove(tarefa);
                SalvarTarefas();
                Console.WriteLine("Tarefa excluída.");
            }
            else
            {
                Console.WriteLine("Tarefa não encontrada.");
            }
        }
    }

    static void SalvarTarefas()
    {
        string json = JsonSerializer.Serialize(tarefas, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    static void CarregarTarefas()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            tarefas = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }
    }
}
