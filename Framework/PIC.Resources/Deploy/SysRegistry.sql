UPDATE SysRegistry SET XData =
N'<Registry>
  <System>
    <Portal>
		<Params>
			<Param Name="LoginPic">
				<Value />
				<Type />
				<DisplayName>登陆图片</DisplayName>
				<Status />
				<Tag />
				<Description />
				<LastModifiedDate />
				<Params />
			</Param>
		</Params>
    </Portal>
    <Modules>
		<Params>
			<Param Name="OA">
				<Value />
				<Type />
				<DisplayName>综合办公</DisplayName>
				<Status />
				<Tag />
				<Description />
				<LastModifiedDate />
				<Params>
					<Param Name="OA.PIM">
						<Value />
						<Type />
						<DisplayName>公共信息管理</DisplayName>
						<Status />
						<Tag />
						<Description />
						<LastModifiedDate />
						<Params>
							<Param Name="OA.PIM.Notice">
								<Value />
								<Type />
								<DisplayName>通知</DisplayName>
								<Status />
								<Tag />
								<Description />
								<LastModifiedDate />
								<Params />
							</Param>
							<Param Name="OA.PIM.News">
								<Value />
								<Type />
								<DisplayName>公司新闻</DisplayName>
								<Status />
								<Tag />
								<Description />
								<LastModifiedDate />
								<Params />
							</Param>
						</Params>
					</Param>
				</Params>
			</Param>
		</Params>
    </Modules>
    <Storage>
    
    </Storage>
    <Workflow>
		
    </Workflow>
    <Params>
		<Param Name="SystemName">
			<Value />
			<Type />
			<DisplayName>系统名</DisplayName>
			<Status />
			<Tag />
			<Description />
			<LastModifiedDate />
		</Param>
		<Param Name="Version">
			<Value />
			<Type />
			<DisplayName>系统版本</DisplayName>
			<Status />
			<Tag />
			<Description />
			<LastModifiedDate />
		</Param>
		<Param Name="CompanyName">
			<Value />
			<Type />
			<DisplayName>公司名</DisplayName>
			<Status />
			<Tag />
			<Description />
			<LastModifiedDate />
		</Param>
		<Param Name="CompanyWebsite">
			<Value />
			<Type />
			<DisplayName>公司网站</DisplayName>
			<Status />
			<Tag />
			<Description />
			<LastModifiedDate />
		</Param>
    </Params>
  </System>
  <Applications>
    <Application Name="Biz">
    </Application>
  </Applications>
</Registry>'

SELECT XData.query('/Registry[1]/Applications[1]/Application[@Name="Biz"]') FROM SysRegistry
SELECT XData.query('/Registry[1]/System[1]/Params') FROM SysRegistry

SELECT LEN(CAST(XData AS Varchar(MAX))) FROM SysRegistry