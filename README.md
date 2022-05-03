<H1>Sapper</H1>

<H3>Simple Library Which Uses Dapper And Makes Easy Crud Operation</H3>

<h6>How To Use</h6>
  
  <h7>Service Registeration</h7>
  
  services.AddScoped<IDapperDriver, DapperDriver>(sp => new DapperDriver(appOptions.connectionString));
  
  <h7>Query</h7>
  <pre>
  var res = await dapperDriver.QueryAsync<User>("SELECT * FROM [Profile].[User]" , System.Data.CommandType.Text);
  </pre>
  
  <h7>Command</h7>
  <pre>
   var res = await dapperDriver.CommandAsync("DELETE FROM Profile.UserFollower", System.Data.CommandType.Text);
            res.ModifiedCount; -- > deleted records count
  </pre>
  
    <h7>StoredProcdure Query</h7>
  <pre>
  var res = await dapperDriver.QueryAsync<SpInputClassModel, SpResultClassModel>(input, "[Profile].[S_User_List]");
  </pre>
  
    <h7>StoredProcdure Command</h7>
  <pre>
  var res = await dapperDriver.CommandAsync(input, "[Profile].[S_User_Save]");
  </pre>
  
    <h7>StoredProcdure Parameter With OutputParams</h7>
  <pre>
  var res = await dapperDriver.CommandAsync(input, "[Profile].[S_User_Save]" , cancellationToken, "Id");
  </pre>
