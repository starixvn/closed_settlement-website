var accMenu = new Array()
//SpecialDelete
accMenu['item_1'] = new Array("&bull; Special Delete|SpecialDelete/Index.aspx",
                                    "&bull; Mix Parlay|SpecialDelete/MP.aspx",
                                    "&bull; Outright|SpecialDelete/Outright.aspx",
                                    "&bull; Update Batch Message|SpecialDelete/UploadBatchMsg.aspx");
//ExchangeRate
accMenu['item_2'] = new Array("&bull; Exchange Rate|ExRate/ExchangeRate.aspx",
                                    "&bull; Min Max Ex.Rate|ExRate/MinMaxExRate.aspx",
                                    "&bull; Invalid Ex.Rate|ExRate/InvalidExRate.aspx");
//Other
accMenu['item_3'] = new Array("&bull; Bet List (IP)|BetList/BetListByIP.aspx?bltype=3&nodate=yes",
                             "&bull; Meeting Commission List|Others/MeetingComm.aspx",
                             "&bull; Casino Sync/Enable Check|Others/CSSyncList.aspx",
                             "&bull; Upload Virtual Rate|Others/UploadVirtualRate.aspx",
                             "&bull; Export Multi Statement|Others/ExportStatement.aspx");
//Settlement
accMenu['item_4'] = new Array("&bull; Calculate|Settle/Index.aspx",
                             "&bull; HT Over Only|Settle/Index.aspx?m=htoveronly",
                             "&bull; Over Only|Settle/Index.aspx?m=overonly",
                             "&bull; Result|Settle/Result.aspx",
                             "&bull; Settlement Log|Settle/GetSettleLog.aspx"
                             );
//User
accMenu['item_8'] = new Array("&bull; Account Management|UserControl/Departments.aspx",
                            "&bull; Add Operator|Customer/AddOperator.aspx",
                            "&bull; Operator List|Customer/OperatorList.aspx",
                            "&nbsp;&nbsp;|UserControl/Function.aspx");
//Customer
accMenu['item_9'] = new Array("&bull; New Customer|Customer/CustomerFrame.aspx",
                            "&bull; Customer List|sMenu9_1", //Contain sub menu
                            "&bull; Edit UserName|Customer/EditUserName.aspx",
                            "&bull; Move Customer|sMenu9_2",
                            "&bull; Move Site|Customer/ChangeSite.aspx",
                            "&bull; User Group|Customer/ChangeUserGroup.aspx",
                            "&bull; Clear Credit|Customer/ChangeUserGroup.aspx?clear=1",
                            "&bull; Auto Suspend|Customer/SuspendCustList.aspx",
                            "&bull; Transfer All|Customer/TransferAllMember.aspx",
                            "&bull; View PT|Customer/ViewPT.aspx",
                            "&bull; View Log|LogDisplay/LogLogin.aspx?tabid=CustomerSetting",
                            "&nbsp;&nbsp;|LogDisplay/LogActionType.aspx");

var sMenu9_1 = new Array()
sMenu9_1 = new Array("&bull; Super List|Customer/SuperAgentList.aspx",
                     "&bull; Master List|Customer/MasterAgentList.aspx?bycom=1");
var sMenu9_2 = new Array()
sMenu9_2 = new Array("&bull; Move Agent|Customer/MoveCustomer.aspx?agent=1",
                      "&bull; Move Master|Customer/MoveCustomer.aspx"
                      );
