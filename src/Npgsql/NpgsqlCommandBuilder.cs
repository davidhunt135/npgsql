﻿using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using NpgsqlTypes;

namespace Npgsql;

///<summary>
/// This class is responsible to create database commands for automatic insert, update and delete operations.
///</summary>
[System.ComponentModel.DesignerCategory("")]
public sealed class NpgsqlCommandBuilder : DbCommandBuilder
{
    // Commented out because SetRowUpdatingHandler() is commented, and causes an "is never used" warning
    // private NpgsqlRowUpdatingEventHandler rowUpdatingHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="NpgsqlCommandBuilder"/> class.
    /// </summary>
    public NpgsqlCommandBuilder()
        : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NpgsqlCommandBuilder"/> class.
    /// </summary>
    /// <param name="adapter">The adapter.</param>
    public NpgsqlCommandBuilder(NpgsqlDataAdapter? adapter)
    {
        DataAdapter = adapter;
        QuotePrefix = "\"";
        QuoteSuffix = "\"";
    }

    /// <summary>
    /// Gets or sets the beginning character or characters to use when specifying database objects (for example, tables or columns) whose names contain characters such as spaces or reserved tokens.
    /// </summary>
    /// <returns>
    /// The beginning character or characters to use. The default is an empty string.
    ///   </returns>
    ///   <PermissionSet>
    ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*" />
    ///   </PermissionSet>
    [AllowNull]
    public override string QuotePrefix
    {
        get => base.QuotePrefix;
        // TODO: Why should it be possible to remove the QuotePrefix?
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                base.QuotePrefix = value;
            }
            else
            {
                base.QuotePrefix = "\"";
            }
        }
    }

    /// <summary>
    /// Gets or sets the ending character or characters to use when specifying database objects (for example, tables or columns) whose names contain characters such as spaces or reserved tokens.
    /// </summary>
    /// <returns>
    /// The ending character or characters to use. The default is an empty string.
    ///   </returns>
    ///   <PermissionSet>
    ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*" />
    ///   </PermissionSet>
    [AllowNull]
    public override string QuoteSuffix
    {
        get => base.QuoteSuffix;
        // TODO: Why should it be possible to remove the QuoteSuffix?
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                base.QuoteSuffix = value;
            }
            else
            {
                base.QuoteSuffix = "\"";
            }
        }
    }

    ///<summary>
    ///
    /// This method is responsible to derive the command parameter list with values obtained from function definition.
    /// It clears the Parameters collection of command. Also, if there is any parameter type which is not supported by Npgsql, an InvalidOperationException will be thrown.
    /// Parameters name will be parameter1, parameter2, ... for CommandType.StoredProcedure and named after the placeholder for CommandType.Text
    ///</summary>
    /// <param name="command">NpgsqlCommand whose function parameters will be obtained.</param>
    public static void DeriveParameters(NpgsqlCommand command) => command.DeriveParameters();

    /// <summary>
    /// Gets the automatically generated <see cref="NpgsqlCommand"/> object required
    /// to perform insertions at the data source.
    /// </summary>
    /// <returns>
    /// The automatically generated <see cref="NpgsqlCommand"/> object required to perform insertions.
    /// </returns>
    public new NpgsqlCommand GetInsertCommand() => GetInsertCommand(false);

    /// <summary>
    /// Gets the automatically generated <see cref="NpgsqlCommand"/> object required to perform insertions
    /// at the data source, optionally using columns for parameter names.
    /// </summary>
    /// <param name="useColumnsForParameterNames">
    /// If <see langword="true"/>, generate parameter names matching column names, if possible.
    /// If <see langword="false"/>, generate <c>@p1</c>, <c>@p2</c>, and so on.
    /// </param>
    /// <returns>
    /// The automatically generated <see cref="NpgsqlCommand"/> object required to perform insertions.
    /// </returns>
    public new NpgsqlCommand GetInsertCommand(bool useColumnsForParameterNames)
    {
        var cmd = (NpgsqlCommand) base.GetInsertCommand(useColumnsForParameterNames);
        cmd.UpdatedRowSource = UpdateRowSource.None;
        return cmd;
    }

    /// <summary>
    /// Gets the automatically generated System.Data.Common.DbCommand object required
    /// to perform updates at the data source.
    /// </summary>
    /// <returns>
    /// The automatically generated System.Data.Common.DbCommand object required to perform updates.
    /// </returns>
    public new NpgsqlCommand GetUpdateCommand() => GetUpdateCommand(false);

    /// <summary>
    /// Gets the automatically generated <see cref="NpgsqlCommand"/> object required to perform updates
    /// at the data source, optionally using columns for parameter names.
    /// </summary>
    /// <param name="useColumnsForParameterNames">
    /// If <see langword="true"/>, generate parameter names matching column names, if possible.
    /// If <see langword="false"/>, generate <c>@p1</c>, <c>@p2</c>, and so on.
    /// </param>
    /// <returns>
    /// The automatically generated <see cref="NpgsqlCommand"/> object required to perform updates.
    /// </returns>
    public new NpgsqlCommand GetUpdateCommand(bool useColumnsForParameterNames)
    {
        var cmd = (NpgsqlCommand)base.GetUpdateCommand(useColumnsForParameterNames);
        cmd.UpdatedRowSource = UpdateRowSource.None;
        return cmd;
    }

    /// <summary>
    /// Gets the automatically generated System.Data.Common.DbCommand object required
    /// to perform deletions at the data source.
    /// </summary>
    /// <returns>
    /// The automatically generated System.Data.Common.DbCommand object required to perform deletions.
    /// </returns>
    public new NpgsqlCommand GetDeleteCommand() => GetDeleteCommand(false);

    /// <summary>
    /// Gets the automatically generated <see cref="NpgsqlCommand"/> object required to perform deletions
    /// at the data source, optionally using columns for parameter names.
    /// </summary>
    /// <param name="useColumnsForParameterNames">
    /// If <see langword="true"/>, generate parameter names matching column names, if possible.
    /// If <see langword="false"/>, generate @p1, @p2, and so on.
    /// </param>
    /// <returns>
    /// The automatically generated <see cref="NpgsqlCommand"/> object required to perform deletions.
    /// </returns>
    public new NpgsqlCommand GetDeleteCommand(bool useColumnsForParameterNames)
    {
        var cmd = (NpgsqlCommand) base.GetDeleteCommand(useColumnsForParameterNames);
        cmd.UpdatedRowSource = UpdateRowSource.None;
        return cmd;
    }

    //never used
    //private string QualifiedTableName(string schema, string tableName)
    //{
    //    if (schema == null || schema.Length == 0)
    //    {
    //        return tableName;
    //    }
    //    else
    //    {
    //        return schema + "." + tableName;
    //    }
    //}

/*
        private static void SetParameterValuesFromRow(NpgsqlCommand command, DataRow row)
        {
            foreach (NpgsqlParameter parameter in command.Parameters)
            {
                parameter.Value = row[parameter.SourceColumn, parameter.SourceVersion];
            }
        }
*/

    /// <summary>
    /// Applies the parameter information.
    /// </summary>
    /// <param name="p">The parameter.</param>
    /// <param name="row">The row.</param>
    /// <param name="statementType">Type of the statement.</param>
    /// <param name="whereClause">If set to <see langword="true"/> [where clause].</param>
    protected override void ApplyParameterInfo(DbParameter p, DataRow row, System.Data.StatementType statementType, bool whereClause)
    {
        var param = (NpgsqlParameter)p;
        param.NpgsqlDbType = (NpgsqlDbType)row[SchemaTableColumn.ProviderType];
    }

    /// <summary>
    /// Returns the name of the specified parameter in the format of @p#.
    /// </summary>
    /// <param name="parameterOrdinal">The number to be included as part of the parameter's name..</param>
    /// <returns>
    /// The name of the parameter with the specified number appended as part of the parameter name.
    /// </returns>
    protected override string GetParameterName(int parameterOrdinal)
        => string.Format(CultureInfo.InvariantCulture, "@p{0}", parameterOrdinal);

    /// <summary>
    /// Returns the full parameter name, given the partial parameter name.
    /// </summary>
    /// <param name="parameterName">The partial name of the parameter.</param>
    /// <returns>
    /// The full parameter name corresponding to the partial parameter name requested.
    /// </returns>
    protected override string GetParameterName(string parameterName)
        => string.Format(CultureInfo.InvariantCulture, "@{0}", parameterName);

    /// <summary>
    /// Returns the placeholder for the parameter in the associated SQL statement.
    /// </summary>
    /// <param name="parameterOrdinal">The number to be included as part of the parameter's name.</param>
    /// <returns>
    /// The name of the parameter with the specified number appended.
    /// </returns>
    protected override string GetParameterPlaceholder(int parameterOrdinal)
        => GetParameterName(parameterOrdinal);

    /// <summary>
    /// Registers the <see cref="NpgsqlCommandBuilder" /> to handle the <see cref="NpgsqlDataAdapter.RowUpdating"/> event for a <see cref="NpgsqlDataAdapter" />.
    /// </summary>
    /// <param name="adapter">The <see cref="System.Data.Common.DbDataAdapter" /> to be used for the update.</param>
    protected override void SetRowUpdatingHandler(DbDataAdapter adapter)
    {
        var npgsqlAdapter = adapter as NpgsqlDataAdapter;
        if (npgsqlAdapter == null)
            throw new ArgumentException("adapter needs to be a NpgsqlDataAdapter", nameof(adapter));

        // Being called twice for the same adapter means unregister
        if (adapter == DataAdapter)
            npgsqlAdapter.RowUpdating -= RowUpdatingHandler;
        else
            npgsqlAdapter.RowUpdating += RowUpdatingHandler;
    }

    /// <summary>
    /// Adds an event handler for the <see cref="NpgsqlDataAdapter.RowUpdating"/> event.
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">A <see cref="NpgsqlRowUpdatingEventArgs"/> instance containing information about the event.</param>
    void RowUpdatingHandler(object sender, NpgsqlRowUpdatingEventArgs e) => base.RowUpdatingHandler(e);

    /// <summary>
    /// Given an unquoted identifier in the correct catalog case, returns the correct quoted form of that identifier, including properly escaping any embedded quotes in the identifier.
    /// </summary>
    /// <param name="unquotedIdentifier">The original unquoted identifier.</param>
    /// <returns>
    /// The quoted version of the identifier. Embedded quotes within the identifier are properly escaped.
    /// </returns>
    /// <PermissionSet>
    ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*" />
    ///   </PermissionSet>
    /// <exception cref="System.ArgumentNullException">Unquoted identifier parameter cannot be null</exception>
    public override string QuoteIdentifier(string unquotedIdentifier)
        => unquotedIdentifier == null
            ? throw new ArgumentNullException(nameof(unquotedIdentifier), "Unquoted identifier parameter cannot be null")
            : $"{QuotePrefix}{unquotedIdentifier.Replace(QuotePrefix, QuotePrefix + QuotePrefix)}{QuoteSuffix}";

    /// <summary>
    /// Given a quoted identifier, returns the correct unquoted form of that identifier, including properly un-escaping any embedded quotes in the identifier.
    /// </summary>
    /// <param name="quotedIdentifier">The identifier that will have its embedded quotes removed.</param>
    /// <returns>
    /// The unquoted identifier, with embedded quotes properly un-escaped.
    /// </returns>
    /// <PermissionSet>
    ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*" />
    ///   </PermissionSet>
    /// <exception cref="System.ArgumentNullException">Quoted identifier parameter cannot be null</exception>
    public override string UnquoteIdentifier(string quotedIdentifier)
    {
        if (quotedIdentifier == null)
            throw new ArgumentNullException(nameof(quotedIdentifier), "Quoted identifier parameter cannot be null");

        var unquotedIdentifier = quotedIdentifier.Trim().Replace(QuotePrefix + QuotePrefix, QuotePrefix);

        if (unquotedIdentifier.StartsWith(QuotePrefix, StringComparison.Ordinal))
            unquotedIdentifier = unquotedIdentifier.Remove(0, 1);

        if (unquotedIdentifier.EndsWith(QuoteSuffix, StringComparison.Ordinal))
            unquotedIdentifier = unquotedIdentifier.Remove(unquotedIdentifier.Length - 1, 1);

        return unquotedIdentifier;
    }
}