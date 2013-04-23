<table>
  <tr>
    <td>
      <table class="tbllayout" style="width:100%">
        <tr class="header01">
          <td colspan="4">ACTIVE ACCOUNT</td>
        </tr>
        <tr class="header01">
          <td class="c" width="80">User Name</td>
          <td class="c" width="130">First Name</td>
          <td class="c" width="130">Last Name</td>
          <td class="c" width="135">Amount</td>
        </tr>
        {activeContent}
        <!--no-actotal-->
        <tr class="report-footer h4bold">
          <td colspan="3" class="r">TOTAL: </td>
          <td class="r">{sumactive}</td>
        </tr>
        <!--/no-actotal-->
      </table>
    </td>
  </tr>
  <tr>
    <td>
      <br/>
    </td>
  </tr>
  <tr>
    <td>
      <table class="tbllayout" style="width:100%">
        <tr class="header01">
          <td colspan="4">INACTIVE ACCOUNT</td>
        </tr>
        <tr class="header01">
          <td class="c" width="80">User Name</td>
          <td class="c" width="130">First Name</td>
          <td class="c" width="130">Last Name</td>
          <td class="c" width="135">Amount</td>
        </tr>
        {inactiveContent}
        <!--no-intotal-->
        <tr class="h4bold report-footer">
          <td colspan="3" class="r">TOTAL: </td>
          <td class="r">{suminactive}</td>
        </tr>
        <!--/no-intotal-->
        <!--no-total-->
        <tr class="r h4bold">
          <td colspan="3">TOTAL Active + Inactive: </td>
          <td>{sumall}</td>
        </tr>
        <!--/no-total-->
      </table>
    </td>
  </tr>
  <tr>
    <td>
      <br/>
    </td>
  </tr>
  <tr>
    <td>
      <input class="btn"  style="width: 250px;" type="submit" id="resetClosed" name="resetClosed" onclick="return resetClosedConfirm();" value="Reset All Closed Account Credit To 0" />
      <input class="btn"  style="width: 250px;" type="submit" id="resetAll" name="resetAll" onclick="return resetAllConfirm();" value="Reset All Account Credit To 0" />
    </td>
  </tr>
</table>