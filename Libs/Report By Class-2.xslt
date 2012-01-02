<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xalan="http://xml.apache.org/xalan" exclude-result-prefixes="xalan">
	<xsl:output method="html" indent="no"/>

	<xsl:template name="substring-after-last">
		<xsl:param name="string" />
		<xsl:param name="char" />
		<xsl:choose>
			<xsl:when test="contains($string, $char)">
				<xsl:call-template name="substring-after-last">
					<xsl:with-param name="string" select="substring-after($string, $char)" />
					<xsl:with-param name="char" select="$char" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise><xsl:value-of select="$string" /></xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="percentage-bar">
		<xsl:param name="length" />
		<xsl:param name="maxVal" />
		<xsl:param name="actualVal" />

		<xsl:choose>
			<xsl:when test="($maxVal+0) &gt; 0">
				<xsl:variable name="prc" select="100 * (1 - $actualVal div $maxVal)" />
				<div class="coverageBar" style="width:{$length}px; position:absolute;">
					<xsl:value-of select="ceiling(100 - $prc)"/>%
				</div>
				<div class="coverageBar" style="background-color:green; width:{$length}px;">					
					<xsl:choose>
						<xsl:when test="$prc &lt; 10">
							<div class="coveragePrc" style="background-color:yellow; width:{$prc}%" />
						</xsl:when>
						<xsl:otherwise>
							<div class="coveragePrc" style="background-color:red; width:{$prc}%" />
						</xsl:otherwise>
					</xsl:choose>					
				</div>
			</xsl:when>
			<xsl:otherwise>
				<div class='coverageBar' style='background-color:whitesmoke; width:{$length}px;' />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<style>
			.assembly {font-weight:bold;}
			.namespace {font-weight:bold; padding-left:10px; }
			.type { padding-left:20px; }
			.method {font-style:italic; padding-left:30px;}
			.methodName {font-weight:bold;}

			.codeSize {display:none;}
			.coveredCodeSize {display:none;}

			.coverageBar {height:15px; color:white; text-align:center;}
			.coveragePrc {height:100%; float:right;}
		</style>


		<table style="border-collapse: collapse;">
			<xsl:for-each select="/PartCoverReport/Assembly">
				<xsl:variable name="asmRefId" select="@id"/>

				<tr>
					<td colspan="2">
						<div class="assembly"><xsl:value-of select="concat(@name, substring-after(@module, @name))" /></div>
					</td>
				</tr>

				<xsl:variable name="AssemblyTypesTmp">
					<xsl:copy-of select="//Type[@asmref=$asmRefId]" />
				</xsl:variable>

				<xsl:variable name="AssemblyTypesUnsorted">
					<xsl:for-each select="xalan:nodeset($AssemblyTypesTmp)/Type">
						<xsl:variable name="typeName">
							<xsl:call-template name="substring-after-last">
								<xsl:with-param name="string" select="@name" />
								<xsl:with-param name="char" select="'.'" />
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="nspace" select="substring(@name, 1, string-length(@name) - string-length($typeName) - 1)" />
						
						<T>
							<FN><xsl:value-of select="@name" /></FN>
							<N><xsl:value-of select="$typeName" /></N>
							<NS><xsl:value-of select="$nspace" /></NS>
							<SZ><xsl:value-of select="sum(./Method/@bodysize)+0" /></SZ>
							<xsl:for-each select="./Method">
								<xsl:sort select="@name" />

								<xsl:variable name="length" select="sum(./pt/@len)+0" />
								<xsl:variable name="lengthV" select="sum(./pt[@visit&gt;0]/@len)+0" />

								<M>
									<N><xsl:value-of select="@name" /></N>
									<SG><xsl:value-of select="@sig" /></SG>
									<L><xsl:value-of select="$length" /></L>
									<V><xsl:value-of select="$lengthV"/></V>
									<SZ><xsl:value-of select="@bodysize+0" /></SZ>
									<xsl:choose>
										<xsl:when test="$length &gt; 0">
											<xsl:variable name="cov" select="$lengthV div $length" />
											<COV><xsl:value-of select="$cov"/></COV>
											<CSZ><xsl:value-of select="@bodysize * $cov"/></CSZ>
										</xsl:when>
										<xsl:otherwise>
											<COV><xsl:value-of select="0"/></COV>
											<CSZ><xsl:value-of select="0"/></CSZ>
										</xsl:otherwise>
									</xsl:choose>
								</M>
							</xsl:for-each>
						</T>
					</xsl:for-each>
				</xsl:variable>
				
				<xsl:variable name="AssemblyTypes">
					<xsl:for-each select="xalan:nodeset($AssemblyTypesUnsorted)/T">
						<xsl:sort select="NS/text()" />						
						<xsl:sort select="N/text()" />
						<xsl:copy-of select="." />
					</xsl:for-each>
				</xsl:variable>

				<xsl:for-each select="xalan:nodeset($AssemblyTypes)/T">
					<xsl:variable name="name" select="FN/text()" />
					<xsl:variable name="typeNamespace" select="NS/text()" />
					<xsl:variable name="prevTypeNamespace" select="concat(preceding-sibling::T[1]/NS/text(),'')" />

					<xsl:if test="$prevTypeNamespace != $typeNamespace">
						<tr>
							<td><div class="namespace"><xsl:value-of select="$typeNamespace" /></div></td>
							<td>
								<xsl:call-template name="percentage-bar">
									<xsl:with-param name="length" select="140" />
									<xsl:with-param name="maxVal" select="sum(following-sibling::T[NS/text()=$typeNamespace]/SZ/text()) + ./SZ/text()" />
									<xsl:with-param name="actualVal" select="sum(following-sibling::T[NS/text()=$typeNamespace]/M/CSZ/text()) + sum(./M/CSZ/text())" />
								</xsl:call-template>
							</td>
						</tr>
					</xsl:if>

					<tr>
						<td><div class="type"><xsl:value-of select="N/text()" /></div></td>
						<td>
							<xsl:call-template name="percentage-bar">
								<xsl:with-param name="length" select="120" />
								<xsl:with-param name="maxVal" select="SZ/text()" />
								<xsl:with-param name="actualVal" select="sum(M/CSZ/text())" />
							</xsl:call-template>
						</td>
					</tr>

					<xsl:for-each select="M[CSZ/text() &gt; 0][CSZ/text() &lt; SZ/text()]">
						<tr>
							<td>
								<div class="method" style="display:none"><xsl:value-of select="N/text()" /></div>
								<div class="method">
									<xsl:value-of select="substring-before(SG/text(), ' (')" />
									<span class="methodName"><xsl:value-of select="N/text()" /></span>						
									<xsl:value-of select="concat('(', substring-after(SG/text(), ' ('))"/>
								</div>
							</td>
							<td>
								<xsl:call-template name="percentage-bar">
									<xsl:with-param name="length" select="100" />
									<xsl:with-param name="maxVal" select="1" />
									<xsl:with-param name="actualVal" select="COV/text()"/>
								</xsl:call-template>
							</td>
						</tr>
					</xsl:for-each>
				</xsl:for-each>
			</xsl:for-each>
		</table>
	</xsl:template>
</xsl:stylesheet>