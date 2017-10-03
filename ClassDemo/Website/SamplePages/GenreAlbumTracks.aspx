<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="GenreAlbumTracks.aspx.cs" Inherits="SamplePages_GenreAlbumTracks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Genre Album Tracks</h1>
    <%-- inside a repeater you need a minimum of 1 item template
        other templates include HeaderTemplate, FooterTemplate,
        AlternatingItemTemplate, SeperatorTemplate
        
        outer repeater will display the first fields from the DTO class 
        which do not repeat (not in a collection)
        outer repeater will get its data from an ODS
        
        nested Repeater will display the collection of the previous repeater
        nested Repeater wil get its data source from the collection of the 
        previous DTO level(either a POCO or another DTO) --%>
    <asp:Repeater ID="GenreAlbumTrackList" runat="server" DataSourceID="GenreAlbumTracksListODS" ItemType="Chinook.Data.DTOs.GenreDTO">
        <ItemTemplate>
            <h2>Genre: <%# Eval("genre") %></h2>  <!--First way of refering to your data in a repeater-->
            <asp:Repeater ID="AlbumTrackList" DataSource='<%#Eval("albums") %>' ItemType="Chinook.Data.DTOs.AlbumDTO" runat="server">
                <ItemTemplate>
                    <h4>Albums:<%# string.Format("{0} ({1}) Tracks: {2}",
                            Eval("title"), Eval("releaseyear"), Eval("Numberoftracks")) %></h4>
                    <!-- Item.Tracks is the second way to refer to inner repeaters data sources-->
                    <asp:Repeater ID="Repeater1" runat="server"
                        DataSource='<%#Item.tracks %>' ItemType="Chinook.Data.POCOs.TrackPOCO"> 
                        
                           <HeaderTemplate>
                               
                                <table>
                               <tr>
                                    <th>
                                        Song
                                    </th>
                                   <th>
                                       Length
                                   </th>
                                </tr>  
                           </HeaderTemplate>
                            
                                 <itemTemplate>
                                 <tr>
                                    <td style="width:600px">
                                        <%#Item.song %>
                                    </td>
                                     <td>
                                        <%#Item.length %>
                                    </td>
                                </tr>
                                
                                    </itemTemplate>
                             <FooterTemplate>
                                </table>
                             </FooterTemplate>   
                            
                            
                        

                    </asp:Repeater>
                </ItemTemplate>
                <SeparatorTemplate>
                    <hr style="height:3px; border:none; color:#000; background-color:#000"; />
                </SeparatorTemplate>
            </asp:Repeater>
        </ItemTemplate>
    </asp:Repeater>
    
    <asp:ObjectDataSource ID="GenreAlbumTracksListODS" runat="server" OldValuesParameterFormatString="original_{0}"
         SelectMethod="Genre_GenreAlbumTracks" TypeName="ChinookSystem.BLL.GenreController">

    </asp:ObjectDataSource>
</asp:Content>

