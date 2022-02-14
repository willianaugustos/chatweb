create schema chat;
use chat;
create table messages(`datetime` TIMESTAMP, `from` varchar(100), `message` varchar(500));
create index ix_datetime on messages(`datetime`);