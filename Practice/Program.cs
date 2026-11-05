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
                EjecutarPruebas();

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
            int[] existencias = { 5, 0, 8 };
            int consultasExitosas = 0;
            int entradasInvalidas = 0;
            int ventasRealizadas = 0;
            int reabastecimientosRealizados = 0;

            Producto gorraAzul = new Producto(5);
            gorraAzul.NumeroModelo = 10;
            gorraAzul.Nombre = "Gorra azul";

            Producto gorraNegra = new Producto(0);
            gorraNegra.NumeroModelo = 25;
            gorraNegra.Nombre = "Gorra negra";

            Producto gorraRoja = new Producto(8);
            gorraRoja.NumeroModelo = 40;
            gorraRoja.Nombre = "Gorra roja";

            Producto[] productos = { gorraRoja, gorraAzul, gorraNegra };

            int modeloBuscado = 25;

            foreach (Producto producto in productos)
            {
                if (producto.NumeroModelo == modeloBuscado)
                {
                    Console.WriteLine(
                        $"Encontrado: {producto.Nombre}, stock: {producto.Stock}");

                    break;
                }
            }

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

                        Console.WriteLine($"Producto seleccionado: {seleccionado.Nombre}");
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

        static bool TryObtenerStock(int[] existencias,int numeroModelo,out int stock)
        {
            stock = 0;

            if (existencias == null ||
                numeroModelo < 1 ||
                numeroModelo > existencias.Length)
            {
                return false;
            }

            int indice = numeroModelo - 1;
            stock = existencias[indice];

            return true;
        }

        static void MostrarResultadoConsulta(bool encontrado, int stock)
        {
            if (!encontrado)
            {
                Console.WriteLine("Modelo no registrado.");
            }
            else if (stock == 0)
            {
                Console.WriteLine("Agotado.");
            }
            else
            {
                Console.WriteLine($"Disponibles: {stock}");
            }
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

        static void ProbarConsulta(string nombrePrueba,int[] existencias,int numeroModelo,bool encontradoEsperado,int stockEsperado)
        {
            try
            {
                int stockReal;
                bool encontradoReal = TryObtenerStock(
                    existencias, numeroModelo, out stockReal);

                bool pruebaCorrecta =
                    encontradoReal == encontradoEsperado &&
                    stockReal == stockEsperado;

                if (pruebaCorrecta)
                {
                    Console.WriteLine($"OK: {nombrePrueba}");
                }
                else
                {
                    Console.WriteLine($"FALLÓ: {nombrePrueba}");

                    Console.WriteLine(
                        $"  Esperado: encontrado={encontradoEsperado}, stock={stockEsperado}");

                    Console.WriteLine(
                        $"  Obtenido: encontrado={encontradoReal}, stock={stockReal}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FALLÓ por excepción: {nombrePrueba}");
                Console.WriteLine($"  Tipo: {ex.GetType().Name}");
                Console.WriteLine($"  Mensaje: {ex.Message}");
            }
        }
        static void ProbarVenta()
        {
            Producto producto = new Producto(5);

            bool primeraVenta = producto.TryVender(3);

            Console.WriteLine(
                $"Primera venta: {primeraVenta}, stock: {producto.Stock}");

            bool segundaVenta = producto.TryVender(4);

            Console.WriteLine(
                $"Segunda venta: {segundaVenta}, stock: {producto.Stock}");

            bool terceraVenta = producto.TryVender(2);

            Console.WriteLine(
                $"Tercera venta: {terceraVenta}, stock: {producto.Stock}");
        }
        static bool ProbarVentaRechazada(int cantidad)
        {
            Producto producto = new Producto(5);

            bool ventaRealizada = producto.TryVender(cantidad);

            bool pruebaCorrecta = ventaRealizada == false && producto.Stock == 5;

            Console.WriteLine(pruebaCorrecta
                    ? $"OK: venta de {cantidad} rechazada sin cambiar stock"
                    : $"FALLÓ: venta de {cantidad}, resultado={ventaRealizada}, stock={producto.Stock}");
            return pruebaCorrecta;
        }

        static bool ProbarVentaDeTodoElStock()
        {
            Producto producto = new Producto(5);

            bool ventaRealizada = producto.TryVender(5);

            bool pruebaCorrecta = ventaRealizada == true && producto.Stock == 0;

            Console.WriteLine(pruebaCorrecta
                    ? "OK: venta de todo el stock"
                    : "FALLÓ: venta de todo el stock");
            return pruebaCorrecta;
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
        static bool ProbarCantidadInvalida()
        {
            Producto producto = new Producto(5);

            ResultadoVenta resultado = producto.Vender(0);

            bool pruebaCorrecta = resultado == ResultadoVenta.CantidadInvalida && producto.Stock == 5;

            Console.WriteLine(pruebaCorrecta
                    ? "OK: cantidad inválida sin cambiar stock"
                    : "FALLÓ: cantidad inválida");
            return pruebaCorrecta;
        }

        static bool ProbarStockInsuficiente()
        {
            Producto producto = new Producto(5);
            ResultadoVenta resultado = producto.Vender(6);

            bool pruebaCorrecta = resultado == ResultadoVenta.StockInsuficiente && producto.Stock == 5;

            Console.WriteLine(pruebaCorrecta
                    ? "OK: stock insuficiente sin cambiar stock"
                    : "FALLÓ: stock insuficiente");
            return pruebaCorrecta;
        }

        static bool ProbarMotivoLimiteStock()
        {
            Producto producto = new Producto(int.MaxValue);

            ResultadoReabastecimiento resultado = producto.Reabastecer(1);

            bool pruebaCorrecta = resultado == ResultadoReabastecimiento.LimiteStockSuperado && producto.Stock == int.MaxValue;

            Console.WriteLine( pruebaCorrecta
                    ? "OK: límite superado sin cambiar stock"
                    : "FALLÓ: límite de stock");
            return pruebaCorrecta;
        }
        static bool ProbarLimiteReabastecimiento()
        {
            Producto producto = new Producto(int.MaxValue - 3);

            bool primerResultado = producto.TryReabastecer(3);

            bool primeraPrueba =
                primerResultado &&
                producto.Stock == int.MaxValue;

            Console.WriteLine(
                primeraPrueba
                    ? "OK: llega al máximo"
                    : "FALLÓ: llega al máximo");

            bool segundoResultado = producto.TryReabastecer(1);

            bool segundaPrueba =
                !segundoResultado &&
                producto.Stock == int.MaxValue;

            Console.WriteLine(
                segundaPrueba
                    ? "OK: rechaza exceso"
                    : "FALLÓ: rechaza exceso");
            return primeraPrueba && segundaPrueba;
        }
        static bool ProbarReabastecimientoYVenta()
        {
            Producto producto = new Producto(0);

            bool reabastecido = producto.TryReabastecer(7);
            bool vendido = producto.TryVender(3);

            bool pruebaCorrecta =
                reabastecido &&
                vendido &&
                producto.Stock == 4;

            Console.WriteLine(
                pruebaCorrecta
                    ? "OK: reabastecer y vender"
                    : "FALLÓ: reabastecer y vender");

            bool reabastecimientoRealizado =
                producto.TryReabastecer(-2);

            bool rechazoCorrecto =
                !reabastecimientoRealizado &&
                producto.Stock == 4;

            Console.WriteLine(
                rechazoCorrecto
                    ? "OK: rechazo sin cambios"
                    : "FALLÓ: rechazo sin cambios");
            return pruebaCorrecta && rechazoCorrecto;
        }
        static void EjecutarPruebas()
        {
            int pruebasCorrectas = 0;
            int pruebasFallidas = 0;
            Console.WriteLine("=== Inicio de pruebas ===");

            bool[] resultados = {EjecutarPruebaSegura("Motivo: cantidad inválida",ProbarCantidadInvalida),EjecutarPruebaSegura("Stock insuficiente",ProbarStockInsuficiente),
                EjecutarPruebaSegura("Motivo: límite de stock",ProbarMotivoLimiteStock),EjecutarPruebaSegura("Límite de reabastecimiento",ProbarLimiteReabastecimiento),
                EjecutarPruebaSegura("Reabastecimiento y venta",ProbarReabastecimientoYVenta),
                EjecutarPruebaSegura("Venta de cero unidades",() => ProbarVentaRechazada(0)),EjecutarPruebaSegura("Venta de cantidad negativa",() => ProbarVentaRechazada(-3)),
                EjecutarPruebaSegura("Venta superior al stock",() => ProbarVentaRechazada(6)),EjecutarPruebaSegura("Venta de todo el stock",ProbarVentaDeTodoElStock)
            };

            foreach (bool paso in resultados)
            {
                if (paso)
                {
                    pruebasCorrectas++;
                }
                else
                {
                    pruebasFallidas++;
                }
            }

            Console.WriteLine("=== Fin de pruebas ===");

            Console.WriteLine($"Pruebas contabilizadas: {pruebasCorrectas + pruebasFallidas}");
            Console.WriteLine($"Correctas: {pruebasCorrectas}");
            Console.WriteLine($"Fallidas: {pruebasFallidas}");
        }
        static bool EjecutarPruebaSegura(string nombre,Func<bool> prueba)
        {
            try
            {
                return prueba();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FALLÓ por excepción: {nombre}");
                Console.WriteLine($"  Tipo: {ex.GetType().Name}");
                Console.WriteLine($"  Mensaje: {ex.Message}");

                return false;
            }
        }
        static bool ProbarExcepcionControlada()
        {
            throw new InvalidOperationException(
                "Excepción deliberada para comprobar el ejecutor.");
        }
    }
}
