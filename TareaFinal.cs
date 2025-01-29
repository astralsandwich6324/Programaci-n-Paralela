using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Program6
{
    static async Task Main()
    {
        Console.WriteLine("Simulación de procesamiento de pedidos en línea\n");

        List<Task> pedidos = new List<Task>();
        CancellationTokenSource cts = new CancellationTokenSource();

        for (int i = 1; i <= 5; i++)
        {
            int pedidoId = i;
            pedidos.Add(ProcesarPedidoAsync(pedidoId, cts.Token));
        }

        
        
        await Task.WhenAny(pedidos);
        Console.WriteLine("\nAl menos un pedido ha sido procesado.");

        Console.WriteLine("Presiona Enter para cancelar los pedidos restantes...");
        Console.ReadLine();
        cts.Cancel();

        await Task.WhenAll(pedidos);
        Console.WriteLine("Todos los pedidos han sido procesados o cancelados.");
    }

    static async Task ProcesarPedidoAsync(int pedidoId, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Pedido {pedidoId} en proceso...");

        try
        {
           
            int delay = new Random().Next(1000, 5000);
            await Task.Delay(delay, cancellationToken);

            
            if (new Random().Next(1, 6) == 1)
            {
                throw new Exception($"Error en el pedido {pedidoId}");
            }

            Console.WriteLine($"Pedido {pedidoId} completado en {delay / 1000.0} segundos.");

        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"Pedido {pedidoId} cancelado.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Pedido {pedidoId} fallido: {ex.Message}");
        }
    }
}
