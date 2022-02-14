create schema chat;
use chat;
create table messages(`MessageId` int NOT NULL AUTO_INCREMENT, `datetime` TIMESTAMP, `from` varchar(100), `message` varchar(500), PRIMARY KEY (MessageId));
create index ix_datetime on messages(`datetime`);