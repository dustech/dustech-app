
select * from auth.user_claim
where user_id <> 'd1ceaeeb-5e1b-4681-8553-b71db016d850';

insert into auth.user_claim (claim_id, user_id, type, value, concurrent_stamp)
select gen_random_uuid (), au.user_id, 'given_name', u.name, gen_random_uuid ()
from  usr."user" u
          join auth."user" au on LEFT(u.name, 1) || u.last_name = au.username
where u.user_id <> 'd1ceaeeb-5e1b-4681-8553-b71db016d850';

insert into auth.user_claim (claim_id, user_id, type, value, concurrent_stamp)
select gen_random_uuid (), au.user_id, 'family_name', u.last_name, gen_random_uuid ()
from  usr."user" u
          join auth."user" au on LEFT(u.name, 1) || u.last_name = au.username
where u.user_id <> 'd1ceaeeb-5e1b-4681-8553-b71db016d850';

insert into auth.user_claim (claim_id, user_id, type, value, concurrent_stamp)
select gen_random_uuid (), au.user_id, 'preferred_username', LEFT(u.name, 1) || u.last_name, gen_random_uuid ()
from  usr."user" u
          join auth."user" au on LEFT(u.name, 1) || u.last_name = au.username
where u.user_id <> 'd1ceaeeb-5e1b-4681-8553-b71db016d850';


insert into auth.user_claim (claim_id, user_id, type, value, concurrent_stamp)
select gen_random_uuid (), au.user_id, 'role', 'Normal', gen_random_uuid ()
from  usr."user" u
          join auth."user" au on LEFT(u.name, 1) || u.last_name = au.username
where u.user_id <> 'd1ceaeeb-5e1b-4681-8553-b71db016d850';



select claim_id, user_id, type, value, concurrent_stamp
from auth.user_claim
where user_id <> 'd1ceaeeb-5e1b-4681-8553-b71db016d850';