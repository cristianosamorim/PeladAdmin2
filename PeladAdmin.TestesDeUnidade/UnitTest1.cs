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
            time.CriarCaixa(new Caixa("Mensalidades 2015", 40, new Vigencia(new DateTime(2015, 1, 1), new DateTime(2015, 12, 31))));
            time.CriarCaixa(new Caixa("Festa final de ano 2015", 10, new Vigencia(new DateTime(2015, 1, 1), new DateTime(2015, 12, 31))));

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

            time.CriarCaixa(new Caixa("Mensalidades 2015", 40, new Vigencia(new DateTime(2015, 1, 1), new DateTime(2015, 12, 31))));
            time.CriarCaixa(new Caixa("Festa final de ano 2015", 10, new Vigencia(new DateTime(2015, 1, 1), new DateTime(2015, 12, 31))));
            time.CriarPeladeiro(new Peladeiro("João Português", new DateTime(2015, 1, 1)));
            time.CriarPeladeiro(new Peladeiro("Cidão", new DateTime(2015, 1, 1)));

            //Act
            ServicoDePagamento servicoDePagamento = new ServicoDePagamento();
            var identificadorDoPagamento = servicoDePagamento.Pagar(time, time.Caixas.First(), time.Peladeiros.First(), new DateTime(2015,1,1), 10);

            //Assert
            Assert.IsTrue(identificadorDoPagamento.Id > 0);
            Assert.IsTrue(identificadorDoPagamento.Valor == -10);
        }

        [TestMethod]
        public void ComoPresidenteQueroInformarUmRecebimento()
        {
            //Arrange
            Time time = new Time("Amigos do Society", new DateTime(2000, 1, 1), new Presida("Marcelo Palladino"));

            time.CriarCaixa(new Caixa("Mensalidades 2015", 40, new Vigencia(new DateTime(2015, 1, 1), new DateTime(2015, 12, 31))));
            time.CriarCaixa(new Caixa("Festa final de ano 2015", 10, new Vigencia(new DateTime(2015, 1, 1), new DateTime(2015, 12, 31))));
            time.CriarPeladeiro(new Peladeiro("João Português", new DateTime(2015, 1, 1)));
            time.CriarPeladeiro(new Peladeiro("Cidão", new DateTime(2015, 1, 1)));

            //Act
            ServicoDeRecebimento servicoDeRecebimento = new ServicoDeRecebimento();
            var identificadorDoRecebimento = servicoDeRecebimento.Receber(time, time.Caixas.First(), time.Peladeiros.First(), new DateTime(2015, 1, 1), 10);

            //Assert
            Assert.IsTrue(identificadorDoRecebimento.Id > 0);
        }

        [TestMethod]
        public void ComoPresidenteQueroVisualizarOsPeladeirosInadimplentes()
        {
            //Arrange
            Time time = new Time("Amigos do Society", new DateTime(2000, 1, 1), new Presida("Marcelo Palladino"));

            //Act
            ServicoDeConsultaDeInadimplentes servicoDeConsultaDeInadimplentes = new ServicoDeConsultaDeInadimplentes();
            IDictionary<Caixa, IEnumerable<Peladeiro>> inadimplentesPorCaixa = servicoDeConsultaDeInadimplentes.Consultar(time, new DateTime(2015,3,10));

            //Assert
            Assert.IsTrue(inadimplentesPorCaixa.Count == 1);
        }
    }

    public class ServicoDeConsultaDeInadimplentes
    {
        public IDictionary<Caixa, IEnumerable<Peladeiro>> Consultar(Time time, DateTime dataDeReferencia)
        {
            return null; //TODO:implementar
        }
    }

    public class ServicoDePagamento
    {
        public LancamentoPagamento Pagar(Time time, Caixa caixa, Peladeiro peladeiro, DateTime dataDePagamento, decimal valor)
        {
            LancamentoPagamento lancamento = new LancamentoPagamento(time, caixa, dataDePagamento, valor);
            return lancamento;
        }
    }

    public class ServicoDeRecebimento
    {
        public LancamentoRecebimento Receber(Time time, Caixa caixa, Peladeiro peladeiro, DateTime dataDeRecebimento, decimal valor)
        {
            LancamentoRecebimento lancamento = new LancamentoRecebimento(time, caixa, peladeiro, dataDeRecebimento, valor);
            return lancamento;
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
        public Caixa(string nome, decimal valorMensalidade, Vigencia vigencia)
        {
            Nome = nome;
            ValorMensalidade = valorMensalidade;
            Vigencia = vigencia;
            LancamentosPagamentos = new List<LancamentoPagamento>();
            LancamentosRecebimentos = new List<LancamentoRecebimento>();
        }

        public IEnumerable<LancamentoRecebimento> LancamentosRecebimentos { get; set; }
        public IEnumerable<LancamentoPagamento> LancamentosPagamentos { get; set; }
        public string Nome { get; private set; }
        public decimal ValorMensalidade { get; private set; }
        public Vigencia Vigencia { get; private set; }
    }

    public class LancamentoPagamento
    {
        public LancamentoPagamento(Time time, Caixa caixa, DateTime data, decimal valor)
        {
            Id = int.MaxValue;
            Time = time;
            Caixa = caixa;
            Data = data;
            Valor = -valor;
        }

        public int Id { get; private set; }
        public Time Time { get; private set; }
        public Caixa Caixa { get; private set; }
        public DateTime Data { get; private set; }
        public decimal Valor { get; private set; }
    }

    public class LancamentoRecebimento
    {
        public LancamentoRecebimento(Time time, Caixa caixa, Peladeiro peladeiro, DateTime data, decimal valor)
        {
            Id = int.MaxValue;
            Time = time;
            Caixa = caixa;
            Peladeiro = peladeiro;
            Data = data;
            Valor = valor;
        }

        public int Id { get; private set; }
        public Time Time { get; private set; }
        public Caixa Caixa { get; private set; }
        public Peladeiro Peladeiro { get; private set; }
        public DateTime Data { get; private set; }
        public decimal Valor { get; private set; }
    }

    public class Vigencia
    {
        public Vigencia(DateTime inicio, DateTime fim)
        {
        }
    }
}
