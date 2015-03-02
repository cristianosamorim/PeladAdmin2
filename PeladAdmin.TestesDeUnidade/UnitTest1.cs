using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PeladAdmin.TestesDeUnidade
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ComoPresidenteQueroCriarUmTime()
        {
            //Arrange
            Time time = new Time("Amigos do Society", new DateTime(2000, 1, 1), new Presida("Marcelo Palladino"));

            //Act
            ServicoDeCriacaoDeTime servicoDeCriacaoDeTime = new ServicoDeCriacaoDeTime();
            var timeCriado = servicoDeCriacaoDeTime.Criar(time);

            //Assert
            Assert.IsTrue(timeCriado.Id > 0);
        }

        [TestMethod]
        public void ComoPresidenteQueroCriarUmOuMaisCaixas()
        {
            //Arrange
            Time time = new Time("Amigos do Society", new DateTime(2000, 1, 1), new Presida("Marcelo Palladino"));

            //Act
            time.CriarCaixa(new Caixa("Mensalidades 2015", 40, new VigenciaDoCaixa(new DateTime(2015, 1, 1), new DateTime(2015, 12, 31))));
            time.CriarCaixa(new Caixa("Festa final de ano 2015", 10, new VigenciaDoCaixa(new DateTime(2015, 1, 1), new DateTime(2015, 12, 31))));

            //Assert
            Assert.IsTrue(time.Caixas.Count() == 2);
        }

        [TestMethod]
        public void ComoPresidenteQueroCriarUmOuMaisPeladeiros()
        {
            //Arrange
            Time time = new Time("Amigos do Society", new DateTime(2000,1,1), new Presida("Marcelo Palladino"));

            //Act
            time.CriarPeladeiro(new Peladeiro("João Português", new DateTime(2015,1,1)));
            time.CriarPeladeiro(new Peladeiro("Cidão", new DateTime(2015,1,1)));

            //Assert
            Assert.IsTrue(time.Peladeiros.Count() == 2);
        }

        [TestMethod]
        public void ComoPresidenteQueroInformarUmPagamento()
        {
            //Arrange
            Time time = new Time("Amigos do Society", new DateTime(2000, 1, 1), new Presida("Marcelo Palladino"));

            time.CriarCaixa(new Caixa("Mensalidades 2015", 40, new VigenciaDoCaixa(new DateTime(2015, 1, 1), new DateTime(2015, 12, 31))));
            time.CriarCaixa(new Caixa("Festa final de ano 2015", 10, new VigenciaDoCaixa(new DateTime(2015, 1, 1), new DateTime(2015, 12, 31))));
            time.CriarPeladeiro(new Peladeiro("João Português", new DateTime(2015, 1, 1)));
            time.CriarPeladeiro(new Peladeiro("Cidão", new DateTime(2015, 1, 1)));

            //Act
            ServicoDePagamento servicoDePagamento = new ServicoDePagamento();
            var resultado = servicoDePagamento.Pagar(time.Caixas.First(), new DateTime(2015,1,1), 10);

            //Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void ComoPresidenteQueroInformarUmRecebimento()
        {
            //Arrange
            Time time = new Time("Amigos do Society", new DateTime(2000, 1, 1), new Presida("Marcelo Palladino"));

            time.CriarCaixa(new Caixa("Mensalidades 2015", 40, new VigenciaDoCaixa(new DateTime(2015, 1, 1), new DateTime(2015, 12, 31))));
            time.CriarCaixa(new Caixa("Festa final de ano 2015", 10, new VigenciaDoCaixa(new DateTime(2015, 1, 1), new DateTime(2015, 12, 31))));
            time.CriarPeladeiro(new Peladeiro("João Português", new DateTime(2015, 1, 1)));
            time.CriarPeladeiro(new Peladeiro("Cidão", new DateTime(2015, 1, 1)));

            //Act
            ServicoDeRecebimento servicoDeRecebimento = new ServicoDeRecebimento();
            var resultado = servicoDeRecebimento.Receber(time.Caixas.First(), time.Peladeiros.First(), new DateTime(2015, 1, 1), 10);

            //Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void ComoPresidenteQueroVisualizarOsPeladeirosInadimplentesQuandoJogadorComecouDepoisDoInicioDaVigenciaDoCaixa()
        {
            //Arrange
            Time time = new Time("Amigos do Society", new DateTime(2000, 1, 1), new Presida("Marcelo Palladino"));
            time.CriarCaixa(new Caixa("Mensalidades 2015", 40, new VigenciaDoCaixa(new DateTime(2015, 1, 1), new DateTime(2015, 12, 31))));
            time.CriarPeladeiro(new Peladeiro("João Português", new DateTime(2015, 2, 1)));
            time.CriarPeladeiro(new Peladeiro("Cidão", new DateTime(2015, 1, 1)));

            var caixa = time.Caixas.First();
            caixa.FazerLancamentoDeRecebimento(time.Peladeiros.First(), DateTime.Now, 160); //o jogador começou em fevereiro

            //Act
            ServicoDeConsultaDeInadimplentes servicoDeConsultaDeInadimplentes = new ServicoDeConsultaDeInadimplentes();
            IDictionary<Caixa, IEnumerable<Peladeiro>> inadimplentesPorCaixa = servicoDeConsultaDeInadimplentes.Consultar(time, new DateTime(2015,5,10));

            //Assert
            Assert.IsTrue(inadimplentesPorCaixa.Count == 1);
            Assert.IsTrue(inadimplentesPorCaixa.First().Value.Count() == 1);
        }

        [TestMethod]
        public void ComoPresidenteQueroVisualizarOsPeladeirosInadimplentesQuandoJogadorComecouAntesDoInicioDaVigenciaDoCaixa()
        {
            //Arrange
            Time time = new Time("Amigos do Society", new DateTime(2000, 1, 1), new Presida("Marcelo Palladino"));
            time.CriarCaixa(new Caixa("Mensalidades 2015", 40, new VigenciaDoCaixa(new DateTime(2015, 1, 1), new DateTime(2015, 12, 31))));
            time.CriarPeladeiro(new Peladeiro("João Português", new DateTime(2015, 1, 1)));
            time.CriarPeladeiro(new Peladeiro("Cidão", new DateTime(2015, 1, 1)));

            var caixa = time.Caixas.First();
            caixa.FazerLancamentoDeRecebimento(time.Peladeiros.First(), DateTime.Now, 200); 

            //Act
            ServicoDeConsultaDeInadimplentes servicoDeConsultaDeInadimplentes = new ServicoDeConsultaDeInadimplentes();
            IDictionary<Caixa, IEnumerable<Peladeiro>> inadimplentesPorCaixa = servicoDeConsultaDeInadimplentes.Consultar(time, new DateTime(2015, 5, 10));

            //Assert
            Assert.IsTrue(inadimplentesPorCaixa.Count == 1);
            Assert.IsTrue(inadimplentesPorCaixa.First().Value.Count() == 1);
        }

        [TestMethod]
        public void ComoPresidenteQueroVisualizarOsPeladeirosInadimplentesQuandoTodosPagaramOQueDeviam()
        {
            //Arrange
            const decimal mensalidade = 40;
            DateTime dataInicioDaVigenciaDoCaixa = new DateTime(2015, 1, 1);
            DateTime dataInicioDeReferenciaParaConsultaDeInadimplentes = new DateTime(2015, 5, 10);


            Time time = new Time("Amigos do Society", new DateTime(2000, 1, 1), new Presida("Marcelo Palladino"));
            time.CriarCaixa(new Caixa("Mensalidades 2015", 40, new VigenciaDoCaixa(dataInicioDaVigenciaDoCaixa, new DateTime(2015, 12, 31))));
            time.CriarPeladeiro(new Peladeiro("João Português", new DateTime(2015, 1, 1)));
            time.CriarPeladeiro(new Peladeiro("Cidão", new DateTime(2015, 1, 1)));

            var caixa = time.Caixas.First();
            caixa.FazerLancamentoDeRecebimento(time.Peladeiros.ElementAt(0), DateTime.Now, 200);
            caixa.FazerLancamentoDeRecebimento(time.Peladeiros.ElementAt(1), DateTime.Now, 200);

            //Act
            ServicoDeConsultaDeInadimplentes servicoDeConsultaDeInadimplentes = new ServicoDeConsultaDeInadimplentes();
            IDictionary<Caixa, IEnumerable<Peladeiro>> inadimplentesPorCaixa = servicoDeConsultaDeInadimplentes.Consultar(time, dataInicioDeReferenciaParaConsultaDeInadimplentes);

            //Assert
            Assert.IsTrue(inadimplentesPorCaixa.Count == 0);
        }
    }

    public class ServicoDeConsultaDeInadimplentes
    {
        public IDictionary<Caixa, IEnumerable<Peladeiro>> Consultar(Time time, DateTime dataDeReferencia)
        {
            var resultado = new Dictionary<Caixa, IEnumerable<Peladeiro>>();

            foreach (var caixa in time.Caixas)
            {
                var peladeirosInadimplementesNesteCaixa = new List<Peladeiro>();

                foreach (var peladeiro in time.Peladeiros)
                {
                    var dataInicialParaApuracaoDeInadimplencia = caixa.VigenciaDoCaixa.PegaDataInicioDeVigenciaConformeDataInicioDoPeladeiro(peladeiro);
                    var dataFinalParaApuracaoDeInadimplencia = dataDeReferencia;

                    if (caixa.CalculaValorPagoPeloPeladeiroComBaseNaDataDeReferencia(peladeiro, dataDeReferencia) < 
                        caixa.CalculaQuantoDeveSerPagoNoPeriodo(dataInicialParaApuracaoDeInadimplencia, dataFinalParaApuracaoDeInadimplencia))
                        peladeirosInadimplementesNesteCaixa.Add(peladeiro);
                }

                if (peladeirosInadimplementesNesteCaixa.Any())
                    resultado.Add(caixa, peladeirosInadimplementesNesteCaixa);
            }

            return resultado;
        }
    }

    public class ServicoDePagamento
    {
        public bool Pagar(Caixa caixa, DateTime dataDePagamento, decimal valor)
        {
            caixa.FazerLancamentoDePagamento(dataDePagamento, valor);
            return true;
        }
    }

    public class ServicoDeRecebimento
    {
        public bool Receber(Caixa caixa, Peladeiro peladeiro, DateTime dataDeRecebimento, decimal valor)
        {
            caixa.FazerLancamentoDeRecebimento(peladeiro, dataDeRecebimento, valor);
            return true;
        }
    }

    public class Time
    {
        public Time(string nome, DateTime dataDeFundacao, Presida presida)
        {
            ValidaCricaoTime(nome, presida);

            Id = int.MaxValue;
            Nome = nome;
            DataDeFundacao = dataDeFundacao;
            Presida = presida;
            Caixas = new List<Caixa>();
            Peladeiros = new List<Peladeiro>();
        }

        public int Id { get; private set; }
        
        public string Nome { get; private set; }
        
        public DateTime DataDeFundacao { get; private set; }
        
        public Presida Presida { get; private set; }
        
        public IEnumerable<Caixa> Caixas { get; set; }

        public IEnumerable<Peladeiro> Peladeiros { get; private set; }
        
        public void CriarCaixa(Caixa caixa)
        {
            ValidaCriacaoCaixa(caixa);
            ((List<Caixa>)Caixas).Add(caixa);
        }

        private void ValidaCricaoTime(string nome, Presida presida)
        {
            if (String.IsNullOrWhiteSpace(nome))
                throw new ArgumentNullException("nome");

            if (presida == null)
                throw new ArgumentNullException("presida");
        }

        private void ValidaCriacaoCaixa(Caixa caixa)
        {
            if (caixa == null)
                throw new ArgumentNullException("caixa");
        }

        public void CriarPeladeiro(Peladeiro peladeiro)
        {
            ValidaCriacaoPeladeiro(peladeiro);
            ((List<Peladeiro>)Peladeiros).Add(peladeiro);
        }
        private void ValidaCriacaoPeladeiro(Peladeiro peladeiro)
        {
            if (peladeiro == null)
                throw new ArgumentNullException("peladeiro");
        }
    }

    public class Presida
    {
        public Presida(string nome)
        {
            Nome = nome;
        }

        public string Nome { get; private set; }
    }

    public class Peladeiro
    {
        public Peladeiro(string nome, DateTime dataDeInicio)
        {
            Nome = nome;
            DataDeInicio = dataDeInicio;
        }

        public string Nome { get; private set; }
        public DateTime DataDeInicio { get; private set; }
    }

    public class ServicoDeCriacaoDeTime
    {
        public Time Criar(Time time)
        {
            return time;
        }
    }

    public class Caixa
    {
        public Caixa(string nome, decimal valorMensalidade, VigenciaDoCaixa vigenciaDoCaixa)
        {
            Nome = nome;
            ValorMensalidade = valorMensalidade;
            VigenciaDoCaixa = vigenciaDoCaixa;
            LancamentosPagamentos = new List<LancamentoPagamento>();
            LancamentosRecebimentos = new List<LancamentoRecebimento>();
        }

        public IEnumerable<LancamentoRecebimento> LancamentosRecebimentos { get; set; }
        public IEnumerable<LancamentoPagamento> LancamentosPagamentos { get; set; }
        public string Nome { get; private set; }
        public decimal ValorMensalidade { get; private set; }
        public VigenciaDoCaixa VigenciaDoCaixa { get; private set; }

        public void FazerLancamentoDeRecebimento(Peladeiro peladeiro, DateTime data, decimal valor)
        {
            ((List<LancamentoRecebimento>)LancamentosRecebimentos).Add(new LancamentoRecebimento(peladeiro, data, valor));
        }

        public void FazerLancamentoDePagamento(DateTime data, decimal valor)
        {
            ((List<LancamentoPagamento>)LancamentosPagamentos).Add(new LancamentoPagamento(data, valor));
        }

        public decimal CalculaQuantoDeveSerPagoNoPeriodo(DateTime dataInicial, DateTime dataFinal)
        {
            return dataInicial.CalcularQuantidadeDeMeses(dataFinal)*ValorMensalidade;
        }

        public decimal CalculaValorPagoPeloPeladeiroComBaseNaDataDeReferencia(Peladeiro peladeiro, DateTime dataDeReferencia)
        {
            return LancamentosRecebimentos.Where(x => x.Peladeiro.Equals(peladeiro) &&
                    x.Data.Date <= dataDeReferencia).Sum(x => x.Valor);
        }
    }

    public abstract class LancamentoBase 
    {
        protected LancamentoBase(DateTime data, decimal valor)
        {
            Data = data;
            Valor = valor;
        }

        public DateTime Data { get; private set; }
        public decimal Valor { get; private set; }
    }

    public class LancamentoPagamento : LancamentoBase //objeto de valor
    {
        public LancamentoPagamento(DateTime data, decimal valor):base(data, valor)
        {
        }
    }

    public class LancamentoRecebimento : LancamentoBase //objeto de valor
    {
        public LancamentoRecebimento(Peladeiro peladeiro, DateTime data, decimal valor):base(data, valor)
        {
            Peladeiro = peladeiro;
        }

        public Peladeiro Peladeiro { get; private set; }
    }

    public class VigenciaDoCaixa //objeto de valor
    {
        public VigenciaDoCaixa(DateTime inicio, DateTime fim)
        {
            Inicio = inicio;
            Fim = fim;
        }

        public DateTime Inicio { get; private set; }
        public DateTime Fim { get; private set; }

        public DateTime PegaDataInicioDeVigenciaConformeDataInicioDoPeladeiro(Peladeiro peladeiro)
        {
            return peladeiro.DataDeInicio.Date > Inicio
                ? peladeiro.DataDeInicio
                : Inicio;
        }
    }

    public static class ExtensoesDeDateTime
    {
        public static int CalcularQuantidadeDeMeses(this DateTime de, DateTime ate)
        {
            return (ate.Month - de.Month) + 1;//todo: pensar em uma solução melhor
        }
    }
}
