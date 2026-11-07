using System;
using System.Collections.Generic;
using System.Text;

namespace Practice
{
    internal static class PruebasInventario
    {
        public static void MostrarInicio()
        {
            System.Console.WriteLine("Preparando las pruebas del inventario.");
        }

        public static void EjecutarPruebas()
        {
            int pruebasCorrectas = 0;
            int pruebasFallidas = 0;
            Console.WriteLine("=== Inicio de pruebas ===");

            bool[] resultados = {EjecutarPruebaSegura("Motivo: cantidad inválida",ProbarCantidadInvalida),EjecutarPruebaSegura("Stock insuficiente",ProbarStockInsuficiente),
                EjecutarPruebaSegura("Motivo: límite de stock",ProbarMotivoLimiteStock),EjecutarPruebaSegura("Límite de reabastecimiento",ProbarLimiteReabastecimiento),
                EjecutarPruebaSegura("Reabastecimiento y venta",ProbarReabastecimientoYVenta),
                EjecutarPruebaSegura("Venta de cero unidades",() => ProbarVentaRechazada(0)),EjecutarPruebaSegura("Venta de cantidad negativa",() => ProbarVentaRechazada(-3)),
                EjecutarPruebaSegura("Venta superior al stock",() => ProbarVentaRechazada(6)),EjecutarPruebaSegura("Venta de todo el stock",ProbarVentaDeTodoElStock),EjecutarPruebaSegura("Stock inicial negativo",ProbarStockInicialNegativo),
                EjecutarPruebaSegura("Stock inicial cero",ProbarStockInicialCero), EjecutarPruebaSegura("Constructor con tres argumentos",ProbarConstructorConTresArgumentos),
                EjecutarPruebaSegura("Modelo cero", ProbarModeloCero), EjecutarPruebaSegura("Primer modelo válido", ProbarPrimerModeloValido),EjecutarPruebaSegura("Nombre con solo espacios", ProbarNombreSoloEspacios)
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
       private static bool EjecutarPruebaSegura(string nombre, Func<bool> prueba)
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
       private static bool ProbarVentaRechazada(int cantidad)
        {
            Producto producto = new Producto(10,"Gorra azul",5);

            bool ventaRealizada = producto.TryVender(cantidad);

            bool pruebaCorrecta = ventaRealizada == false && producto.Stock == 5;

            Console.WriteLine(pruebaCorrecta ? $"OK: venta de {cantidad} rechazada sin cambiar stock" : $"FALLÓ: venta de {cantidad}, resultado={ventaRealizada}, stock={producto.Stock}");
            return pruebaCorrecta;
        }
       private static bool ProbarVentaDeTodoElStock()
        {
            Producto producto = new Producto(10, "Gorra azul", 5);

            bool ventaRealizada = producto.TryVender(5);

            bool pruebaCorrecta = ventaRealizada == true && producto.Stock == 0;

            Console.WriteLine(pruebaCorrecta ? "OK: venta de todo el stock" : "FALLÓ: venta de todo el stock");
            return pruebaCorrecta;
        }
       private static bool ProbarCantidadInvalida()
        {
            Producto producto = new Producto(10, "Gorra azul", 5);

            ResultadoVenta resultado = producto.Vender(0);

            bool pruebaCorrecta = resultado == ResultadoVenta.CantidadInvalida && producto.Stock == 5;

            Console.WriteLine(pruebaCorrecta ? "OK: cantidad inválida sin cambiar stock" : "FALLÓ: cantidad inválida");
            return pruebaCorrecta;
        }
        private static bool ProbarStockInsuficiente()
        {
            Producto producto = new Producto(10, "Gorra azul", 5);
            ResultadoVenta resultado = producto.Vender(6);

            bool pruebaCorrecta = resultado == ResultadoVenta.StockInsuficiente && producto.Stock == 5;

            Console.WriteLine(pruebaCorrecta ? "OK: stock insuficiente sin cambiar stock" : "FALLÓ: stock insuficiente");
            return pruebaCorrecta;
        }
       private static bool ProbarMotivoLimiteStock()
        {
            Producto producto = new Producto(10,"Gorra azul",int.MaxValue);

            ResultadoReabastecimiento resultado = producto.Reabastecer(1);

            bool pruebaCorrecta = resultado == ResultadoReabastecimiento.LimiteStockSuperado && producto.Stock == int.MaxValue;

            Console.WriteLine(pruebaCorrecta ? "OK: límite superado sin cambiar stock": "FALLÓ: límite de stock");
            return pruebaCorrecta;
        }
       private static bool ProbarLimiteReabastecimiento()
        {
            Producto producto = new Producto(10, "Gorra azul", int.MaxValue - 3);

            bool primerResultado = producto.TryReabastecer(3);

            bool primeraPrueba = primerResultado && producto.Stock == int.MaxValue;

            Console.WriteLine(primeraPrueba ? "OK: llega al máximo" : "FALLÓ: llega al máximo");

            bool segundoResultado = producto.TryReabastecer(1);

            bool segundaPrueba = !segundoResultado && producto.Stock == int.MaxValue;

            Console.WriteLine(segundaPrueba ? "OK: rechaza exceso" : "FALLÓ: rechaza exceso");
            return primeraPrueba && segundaPrueba;
        }
       private static bool ProbarReabastecimientoYVenta()
        {
            Producto producto = new Producto(10, "Gorra azul", 0);

            bool reabastecido = producto.TryReabastecer(7);
            bool vendido = producto.TryVender(3);

            bool pruebaCorrecta = reabastecido && vendido && producto.Stock == 4;

            Console.WriteLine(pruebaCorrecta ? "OK: reabastecer y vender" : "FALLÓ: reabastecer y vender");

            bool reabastecimientoRealizado = producto.TryReabastecer(-2);

            bool rechazoCorrecto = !reabastecimientoRealizado && producto.Stock == 4;

            Console.WriteLine(rechazoCorrecto ? "OK: rechazo sin cambios" : "FALLÓ: rechazo sin cambios");
            return pruebaCorrecta && rechazoCorrecto;
        }
        private static bool ProbarStockInicialNegativo()
        {
            try
            {
                Producto producto = new Producto(10, "Gorra azul", -1);

                Console.WriteLine("FALLÓ: se permitió crear un producto con stock negativo.");

                return false;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("OK: el constructor rechazó el stock negativo.");

                return true;
            }
        }
        private static bool ProbarStockInicialCero()
        {
            Producto producto = new Producto(10, "Gorra azul", 0);

            bool pruebaCorrecta = producto.Stock == 0;

            Console.WriteLine(pruebaCorrecta ? "OK: se permite un producto con stock cero." : "FALLÓ: el stock inicial debería ser cero.");

            return pruebaCorrecta;
        }
        private static bool ProbarConstructorConTresArgumentos()
        {
            Producto producto = new Producto(25, "Gorra negra", 0);
            bool pruebaCorrecta = producto.NumeroModelo == 25 && producto.Nombre == "Gorra negra" && producto.Stock == 0;
            Console.WriteLine(pruebaCorrecta ? "OK: el constructor asignó modelo, nombre y stock." : "FALLÓ: el producto no tiene los datos esperados.");
            return pruebaCorrecta;
        }
        private static bool ProbarModeloCero()
        {
            try
            {
                new Producto(0, "Gorra azul", 0);
                return false;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return ex.ParamName == "numeroModelo";
            }
        }
        private static bool ProbarPrimerModeloValido()
        {
            Producto producto = new Producto(1, "Gorra azul", 0);

            return producto.NumeroModelo == 1 && producto.Stock == 0;
        }
        private static bool ProbarNombreSoloEspacios()
        {
            try
            {
                new Producto(25, "   ", 0);
                return false; // Si llegó aquí, aceptó un nombre formado solo por espacios.
            }
            catch (ArgumentException ex)
            {
                return ex.ParamName == "nombre";
            }
        }
    }
}
