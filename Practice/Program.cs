using Practice;
using System;

namespace CursoRicardo.Depuracion
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("1 = ejecutar pruebas");
            Console.WriteLine("2 = usar inventario");
            Console.Write("Elige una opción: ");

            string modo = Console.ReadLine();

            if (modo == "1")
            {
                PruebasInventario.EjecutarPruebas();

                Console.WriteLine("Presiona Enter para cerrar.");
                Console.ReadLine();
                return;
            }

            if (modo != "2")
            {
                Console.WriteLine("Opción inválida.");
                Console.ReadLine();
                return;
            }
            //int[] existencias = { 5, 0, 8 };
            int consultasExitosas = 0;
            int entradasInvalidas = 0;
            int ventasRealizadas = 0;
            int reabastecimientosRealizados = 0;

            Producto[] productos = CrearCatalogo();

            while (true)
            {
                Console.Write("Número de modelo o salir: ");
                string entrada = Console.ReadLine();

                if (entrada == "salir" || entrada == null)
                {
                    break;
                }

                bool convertido = int.TryParse(entrada, out int numeroModelo);

                if (!convertido)
                {
                    entradasInvalidas++;
                    Console.WriteLine(
                        "Escribe el número del modelo, por ejemplo: 2.");
                }
                else
                {
                    Producto seleccionado;

                    bool encontrado = TryObtenerProducto(productos, numeroModelo, out seleccionado);

                    if (!encontrado)
                    {
                        Console.WriteLine("Modelo no registrado.");
                    }
                    else
                    {
                        consultasExitosas++;

                        Console.WriteLine($"Producto seleccionado: {seleccionado.Nombre}. Stock actual: {seleccionado.Stock}");
                        Console.Write("Operación: 1 = vender, 2 = reabastecer: ");
                        string opcion = Console.ReadLine();

                        if (opcion == "1")
                        {
                            bool ventaRealizada = SolicitarVenta(seleccionado);

                            if (ventaRealizada)
                            {
                                ventasRealizadas++;
                            }
                        }
                        else if (opcion == "2")
                        {
                            bool realizado = SolicitarReabastecimiento(seleccionado);

                            if (realizado)
                            {
                                reabastecimientosRealizados++;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Opción inválida. Volverás a elegir un modelo.");
                        }
                    }
                }
            }
            Console.WriteLine($"Reabastecimientos realizados: {reabastecimientosRealizados}");
            Console.WriteLine($"Consultas exitosas: {consultasExitosas}");
            Console.WriteLine($"Entradas de modelo inválidas: {entradasInvalidas}");
            Console.WriteLine($"Ventas realizadas: {ventasRealizadas}");
            Console.WriteLine("Consultas terminadas.");
            Console.ReadLine();
        }

        static bool TryObtenerProducto(Producto[] productos,int numeroModelo,out Producto productoEncontrado)
        {
            productoEncontrado = null;

            if (productos == null)
            {
                return false;
            }

            foreach (Producto producto in productos)
            {
                if (producto != null && producto.NumeroModelo == numeroModelo)
                {
                    productoEncontrado = producto;
                    return true;
                }
            }

            return false;
        }

        static bool TryAgregarProducto(Producto[] productos, Producto nuevo)
        {
            if (productos == null || nuevo == null)
                return false;

            if (TryObtenerProducto(productos, nuevo.NumeroModelo, out _))
                return false;

            for (int i = 0; i < productos.Length; i++)
            {
                if (productos[i] == null)
                {
                    productos[i] = nuevo;
                    return true;
                }
            }

            return false; // El arreglo está lleno.
        }

        static bool SolicitarVenta(Producto producto)
        {
            Console.WriteLine($"Producto: {producto.Nombre}. Stock actual: {producto.Stock}");

            Console.Write("Cantidad a vender: ");
            string entradaCantidad = Console.ReadLine();

            bool convertido = int.TryParse(entradaCantidad, out int cantidad);

            if (!convertido)
            {
                Console.WriteLine("Cantidad inválida: escribe un número entero.");
                return false;
            }

            ResultadoVenta resultado = producto.Vender(cantidad);

            if (resultado == ResultadoVenta.Realizada)
            {
                Console.WriteLine($"Venta realizada. Stock restante: {producto.Stock}");
            }
            else if (resultado == ResultadoVenta.CantidadInvalida)
            {
                Console.WriteLine("La cantidad debe ser mayor que cero.");
            }
            else if (resultado == ResultadoVenta.StockInsuficiente)
            {
                Console.WriteLine($"Stock insuficiente. Disponibles: {producto.Stock}");
            }

            return resultado == ResultadoVenta.Realizada;
        }
        static bool SolicitarReabastecimiento(Producto producto)
        {
            Console.WriteLine($"Producto: {producto.Nombre}. Stock actual: {producto.Stock}");

            Console.Write("Cantidad a recibir: ");
            string entrada = Console.ReadLine();

            bool convertido = int.TryParse(entrada, out int cantidad);

            if (!convertido)
            {
                Console.WriteLine("Cantidad inválida: escribe un número entero.");
                return false;
            }

            ResultadoReabastecimiento resultado = producto.Reabastecer(cantidad);

            if (resultado == ResultadoReabastecimiento.Realizado)
            {
                Console.WriteLine($"Reabastecimiento realizado. Stock actual: {producto.Stock}");
            }
            else if (resultado == ResultadoReabastecimiento.CantidadInvalida)
            {
                Console.WriteLine("La cantidad debe ser mayor que cero.");
            }
            else if (resultado == ResultadoReabastecimiento.LimiteStockSuperado)
            {
                Console.WriteLine("La cantidad supera el límite de stock permitido.");
            }

            return resultado == ResultadoReabastecimiento.Realizado;
        }
        static Producto[] CrearCatalogo()
        {
            Producto gorraAzul = new Producto(10, "Gorra azul", 5);

            Producto gorraNegra = new Producto(25, "Gorra negra", 0);

            Producto gorraRoja = new Producto(40, "Gorra roja", 8);

            return new Producto[] { gorraRoja, gorraAzul, gorraNegra };
        }
    }
}
