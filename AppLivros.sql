create table tb_livros
(
id int auto_increment primary key,
titulo varchar(500) not null,
autor varchar(150) not null,
ano_publicacao datetime default current_timestamp,
genero varchar(150) not null,
numero_paginas char(15)
);

select * from tb_livros;

insert into tb_livros(titulo,autor,ano_publicacao,genero,numero_paginas)
values ('Mesa','Don Juan','2024/06/07','Nsei','15 páginas');