/*************************************************************
** Class generated by CodeTrigger, Version 6.3.0.5
** This class was generated on 28/05/2021 14:51:27
**************************************************************/

using System;

namespace SSXImport.Data.Interfaces
{
	public interface IDataCriterion
	{
		string ToSql();
	}

	public interface IDataOrderBy
	{
		string ToSql();
	}

	public interface IDataSkip
	{
		string ToSql();
	}

	public interface IDataTake
	{
		string ToSql();
	}

	public interface IDataProjection
	{
		string Member { get; set; }
		string ToSql();
	}

}