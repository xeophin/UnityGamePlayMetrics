<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet exclude-result-prefixes="xs xd" version="2.0"
    xmlns:xd="http://www.oxygenxml.com/ns/doc/xsl"
    xmlns:xs="http://www.w3.org/2001/XMLSchema"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xd:doc scope="stylesheet">
        <xd:desc>
            <xd:p><xd:b>Created on:</xd:b> Oct 1, 2010</xd:p>
            <xd:p><xd:b>Author:</xd:b> kmanz</xd:p>
            <xd:p/>
        </xd:desc>
    </xd:doc>
    <xsl:output method="text"/>
    <xsl:strip-space elements="*"/>
    
    <xsl:variable name="starttime" select="/trackerdata/tracking/location/eventname[text()='start-playing']/../@runningTime" />
    
    <xsl:template match="/">

        <xsl:text>seconds,event,x,y,z&#xA;</xsl:text>
        <xsl:apply-templates select="trackerdata/meta/calibration"/>
        <xsl:apply-templates select="/trackerdata/tracking/location"/>
    </xsl:template>


    <xsl:template match="trackerdata/meta/calibration">
        <xsl:text>,calibration,</xsl:text>
        <xsl:value-of select="x"/>,<xsl:value-of select="y"/>,<xsl:value-of
            select="z"/>
        <xsl:text>&#xA;</xsl:text>
    </xsl:template>

    <xsl:template match="/trackerdata/tracking/location">
        <xsl:value-of select="@runningTime - $starttime" />
        <xsl:text>,</xsl:text>
        <xsl:value-of select="./*" separator="," />
        <xsl:text>&#xA;</xsl:text>
    </xsl:template>

</xsl:stylesheet>