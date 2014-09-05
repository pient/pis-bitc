
-- 查询不同步的人员组织结构
Select 
	ou.UserID, ou.WorkNo, ou.Name, 
	ou.ReportToID, og.PrincipalID, ou.ReportToName, og.PrincipalName, 
	ou.DeptID, og.GroupID, ou.DeptName, og.Name GroupName 
--update OrgUser set ReportToID = og.PrincipalID, ReportToName = og.PrincipalName,
--OrgUser.DeptID = og.GroupID, OrgUser.DeptName = og.Name
from 
(
	select WorkNo, UserID, UserName, MAX(GroupId) GroupId, MAX(GroupCode) GroupCode, MAX(GroupName) GroupName from vw_OrgUserGroup
	where RoleCode != 'Sys_User'
	group by WorkNo, UserID, UserName having COUNT(1) = 1
) gu
left join OrgGroup og on gu.GroupId = og.GroupID
right join OrgUser ou on ou.UserID = gu.UserID
where (ReportToID != PrincipalID or ReportToName != PrincipalName
or DeptID != og.GroupID or DeptName != og.Name)