create table _GraphFilePaths(GraphFilePath nvarchar(150), NumberOfEdges integer, NumberOfPartitions integer, Alfa float, Beta float, Ro float, Delta float, TimesToRun integer);
--insert into  _GraphFilePaths (GraphFilePath, NumberOfPartitions, NumberOfEdges, Alfa, Beta, Ro, Delta, TimesToRun) values ('myciel4.col', 2, 71, 1, 5, 0.6, 0.1, 10);
--insert into  _GraphFilePaths (GraphFilePath, NumberOfPartitions, NumberOfEdges, Alfa, Beta, Ro, Delta, TimesToRun) values ('queen5_5.col', 2, 160, 1, 5, 0.6, 0.1, 10);

--insert into  _GraphFilePaths (GraphFilePath, NumberOfPartitions, NumberOfEdges, Alfa, Beta, Ro, Delta, TimesToRun) values ('myciel5.col', 3, 236, 1, 5, 0.6, 0.1, 10);
--insert into  _GraphFilePaths (GraphFilePath, NumberOfPartitions, NumberOfEdges, Alfa, Beta, Ro, Delta, TimesToRun) values ('miles500.col', 8, 1170, 1, 5, 0.6, 0.1, 10);
--insert into  _GraphFilePaths (GraphFilePath, NumberOfPartitions, NumberOfEdges, Alfa, Beta, Ro, Delta, TimesToRun) values ('queen12_12.col', 9, 2596, 1, 5, 0.6, 0.1, 10);
--insert into  _GraphFilePaths (GraphFilePath, NumberOfPartitions, NumberOfEdges, Alfa, Beta, Ro, Delta, TimesToRun) values ('myciel7.col', 12, 2360, 1, 5, 0.6, 0.1, 10);
--insert into  _GraphFilePaths (GraphFilePath, NumberOfPartitions, NumberOfEdges, Alfa, Beta, Ro, Delta, TimesToRun) values ('le450_5a.col', 29, 5714, 1, 5, 0.6, 0.1, 10);

insert into  _GraphFilePaths (GraphFilePath, NumberOfPartitions, NumberOfEdges, Alfa, Beta, Ro, Delta, TimesToRun) values ('DSJC1000.1.col', 63, 40629, 1, 5, 0.6, 0.1, 10);
insert into  _GraphFilePaths (GraphFilePath, NumberOfPartitions, NumberOfEdges, Alfa, Beta, Ro, Delta, TimesToRun) values ('DSJC125.1.col', 8, 736, 1, 5, 0.6, 0.1, 10);
insert into  _GraphFilePaths (GraphFilePath, NumberOfPartitions, NumberOfEdges, Alfa, Beta, Ro, Delta, TimesToRun) values ('flat1000_50_0.col', 63, 245000, 1, 5, 0.6, 0.1, 10);
insert into  _GraphFilePaths (GraphFilePath, NumberOfPartitions, NumberOfEdges, Alfa, Beta, Ro, Delta, TimesToRun) values ('mulsol.i.1.col', 13, 3925, 1, 5, 0.6, 0.1, 10);
insert into  _GraphFilePaths (GraphFilePath, NumberOfPartitions, NumberOfEdges, Alfa, Beta, Ro, Delta, TimesToRun) values ('queen16_16.col', 16, 6320, 1, 5, 0.6, 0.1, 10);

create table _VarientData(NumberOfIterations integer);
insert into _VarientData(NumberOfIterations)values(100);
insert into _VarientData(NumberOfIterations)values(1000);
insert into _VarientData(NumberOfIterations)values(3000);

delete from AnalyzeData;
delete from AnalyzeResults;

insert into AnalyzeData(GraphFilePath, NumberOfPartitions, NumberOfEdges, Alfa, Beta, Ro, Delta, NumberOfIterations, TimesToRun)
select g.GraphFilePath, g.NumberOfPartitions, g.NumberOfEdges, g.Alfa, g.Beta, g.Ro, g.Delta, v.NumberOfIterations, g.TimesToRun
from _GraphFilePaths as g, _VarientData as v

drop table _GraphFilePaths;
drop table _VarientData;