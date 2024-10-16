
public class Funcionario
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string CPF { get; set; }
    public decimal Salario { get; set; }
}

public class FolhaPagamento
{
    public int Id { get; set; }
    public int FuncionarioId { get; set; }
    public decimal Valor { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    
    public Funcionario Funcionario { get; set; }
}
