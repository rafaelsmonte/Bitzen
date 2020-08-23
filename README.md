./PostmanCollection : Contem os arquivos de teste do postman

./DB/Docker: Contem o arquivo para subir o container do SQLServer    

./DB/scriptBanco.txt: Contem o script para gerar o banco de dados  

------------------------------------------------------  

Para conexão com o banco é necessário um arquivo .env na raiz do programa com as seguintes variáveis 

USER=Usuario do SQLSERVER  
PASSWORD= Senha  
SERVER= Server   
DATABASE= Database   
SECRET= Segredo necessario para gerar senha dos usuarios e JWT  
------------------------------------------------------  

A aplicação é feita no padrão REST com os Status code adequados para cada situação


./Models/User.cs   
ID: Chave primaria com identity.  
Nome: Deve conter entre 3 e 100 caracteres  
Email: É único e é validade se é um email válido.  
Senha: É requerida e deve conter de 3 à 20 caracteres.  
	A senha é encriptada quanto jogada ao banco com SHA256  

./Models/Veiculos.cs  
ID: Chave primaria com identity.  
Modelo: Deve conter entre 3 e 100 caracteres  
Marca: Deve conter entre 3 e 50 caracteres  
Ano: Deve ser entre 1 e o ano atual +  um  
Placa: Deve conter 7 digitos  
TipoDoVeiculo: Requerido  
TipoDeCombustivel: Requerido  
KM: Quilometragem de quando o veiculo foi inserido, tem que ser maior que 1.  
Responsavel: O usuario responsável pelo veiculo  

./Models/Abastecimento.cs   
ID: Chave primaria com identity.  
KmDoAbastecimento: Deve ser maior que zero e maior do que a ultima quilimetragem registrada num abastecimento  
ValorPago: Deve ser maior que zero  
LitrosAbastecidos: Deve ser maior que zero  
DataDoAbastecimento: Data automatica dia do abastecimento  
Posto: Deve ter entre 1 e 100 caracteres  
Responsavel: Responsavel pelo abastecimento  
TipoDoCombustivel: Requerido  
Veiculo: Veiculo que foi abastecido  
###Rotas  
User: v1/user . Aceita Put,post e delete. Por motivos de segurança o GET de todos usuario não existe  
Veiculo: v1/veiculo. Aceita Put,post ,delete e Get. Todos só retornam informações referente ao usuario logado.  
Abastecimento: v1/abastecimento. Aceita Put,post ,delete e Get. Todos só retornam informações referente ao usuario logado.  
Relatorios: v1/relatorios/Nome do Relatorio/Ano: Retorna o relatorio referente ao ano informado  

## Controlers  
Para o uso dos controllers é necessário que o usuário esteja autenticado usando um JWT no formato: bearer + token, esse token é fornecido pela aplicação quanto o usuario efetua o login e tem duração de 1 dia.  
O controler de Usuario aceita requisições anonimas, ou seja, sem autenticação, somente no POST(Create e Auth)  
Para o controler de usuarios, os mesmo só podem deletar e exlcuir se estiverem logados no sistema e essas açõs só são permitidas neles mesmos

Os controlers de Relatorio, Veiculo e Abastecimento só aceitam requisiçoes que sao direcionadas ao usuario logado, ou seja, um veiculo só poder ser alterado ou excluido pelo seu usuario responsável. Isso também ocorrer no abastecimento e nos relatorios.  

###Relatorios   
Os 4 realtorios fornecem um arquivo JSON como resultado  
AbastecidoPorAno/{ano:int} Retorna todos os abastecimento do usuario logado naquele ano e soma quantos litros foram abastecidos.  
 	    Mes =  O mes referente aos abastecimentos   
                    Veiculo = Codigo do  veiculo   
                    TotalDeLitros = Total de litros abastecidos anaquele mes  
PagoPorAno/{ano:int} Retorna todos os abastecimento do usuario logado naquele ano e soma to total pago.  
 	    Mes =  O mes referente aos abastecimentos   
                    Veiculo = Codigo do  veiculo   
                    TotalPago= Total pago anaquele mes  
KmRodadoPorAno/{ano:int} Retona os KMs rodados pelo usuario logado naquele ano   
	  Mes =  O mes referente aos abastecimentos   
	  KMRodados = KM rodados naquele mes  

MediaMensal/{ano:int} Retona a media mensal que foi feita por mes  
	  Mes =  O mes referente aos abastecimentos   
	  TotalKMPorLitro= Consumo medio do veiculo  


 











