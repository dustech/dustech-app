
select u.user_id,
       sub,
       username,
       password,
       active,
       u.concurrent_stamp,
       claim_id,
       uc.user_id,
       type,
       value,
       uc.concurrent_stamp
from auth."user" as u
         left join auth.user_claim as uc
                   on u.user_id = uc.user_id;

insert into auth.user (user_id, sub, username, password, active, concurrent_stamp)
select gen_random_uuid (),
       uu.user_id,LEFT(uu.name, 1) || uu.last_name AS username,
       'password',
       true,
       gen_random_uuid ()
from "usr"."user" as uu
where uu.last_name <> 'STesla'
;

select u.user_id,
       sub,
       username,
       password,
       active,
       u.concurrent_stamp,
       claim_id,
       uc.user_id,
       type,
       value,
       uc.concurrent_stamp
from auth."user" as u
         left join auth.user_claim as uc
                   on u.user_id = uc.user_id;