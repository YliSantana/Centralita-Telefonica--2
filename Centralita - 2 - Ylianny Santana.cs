using System;
using System.Collections.Generic;

namespace CentralitaTelefonicaRD
{
    public abstract class Llamada
    {
        protected string numOrigen;
        protected string numDestino;
        protected double duracion;

        public Llamada(string numOrigen, string numDestino, double duracion)
        {
            this.numOrigen = numOrigen;
            this.numDestino = numDestino;
            this.duracion = duracion;
        }

        public string GetNumOrigen() => numOrigen;
        public string GetNumDestino() => numDestino;
        public double GetDuracion() => duracion;

        public abstract double CalcularPrecio();

        public override string ToString()
        {
            return $"Origen: {numOrigen}, Destino: {numDestino}, Duración: {duracion} seg.";
        }
    }

    public class LlamadaLocal : Llamada
    {
        private double precio = 2.50; // Precio por minuto en pesos dominicanos

        public LlamadaLocal(string numOrigen, string numDestino, double duracion)
            : base(numOrigen, numDestino, duracion) { }

        public override double CalcularPrecio()
        {
            return (duracion / 60) * precio; 
        }

        public override string ToString()
        {
            return base.ToString() + $" (Local) - Precio: {CalcularPrecio():0.00} DOP";
        }
    }

    public class LlamadaProvincial : Llamada
    {
        private static readonly double[] tarifas = { 3.00, 4.50, 6.00 }; // Tarifas por minuto en DOP según franja
        private int franjaHoraria;

        public LlamadaProvincial(string numOrigen, string numDestino, double duracion, int franjaHoraria)
            : base(numOrigen, numDestino, duracion)
        {
            this.franjaHoraria = franjaHoraria;
        }

        public override double CalcularPrecio()
        {
            return (duracion / 60) * tarifas[franjaHoraria - 1];
        }

        public override string ToString()
        {
            return base.ToString() + $" (Provincial - Franja {franjaHoraria}) - Precio: {CalcularPrecio():0.00} DOP";
        }
    }

    public class Centralita
    {
        private List<Llamada> llamadas = new List<Llamada>();
        private double totalFacturado = 0;

        public void RegistrarLlamada(Llamada llamada)
        {
            llamadas.Add(llamada);
            totalFacturado += llamada.CalcularPrecio();
        }

        public int GetTotalLlamadas() => llamadas.Count;
        public double GetTotalFacturado() => totalFacturado;

        public void MostrarLlamadas()
        {
            Console.WriteLine("Registro de Llamadas:");
            foreach (var llamada in llamadas)
            {
                Console.WriteLine(llamada);
            }
        }
    }

    class Practica2
    {
        static void Main()
        {
            Centralita centralita = new Centralita();

            centralita.RegistrarLlamada(new LlamadaLocal("8091234567", "8097654321", 120));
            centralita.RegistrarLlamada(new LlamadaProvincial("8291234567", "8497654321", 180, 1));
            centralita.RegistrarLlamada(new LlamadaProvincial("8491234567", "8097654321", 90, 2));
            centralita.RegistrarLlamada(new LlamadaLocal("8092345678", "8298765432", 45));

            centralita.MostrarLlamadas();

            Console.WriteLine($"\nTotal de llamadas: {centralita.GetTotalLlamadas()}");
            Console.WriteLine($"Total facturado: {centralita.GetTotalFacturado():0.00} DOP");
        }
    }
}